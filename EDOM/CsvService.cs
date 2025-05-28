using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace EDOM
{
    public class CsvService
    {
        public static List<SurveyResponse> ReadCsv(string filePath, out string[] questionHeaders)
        {
            var responses = new List<SurveyResponse>();
            questionHeaders = Array.Empty<string>();

            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();
                    questionHeaders = csv.HeaderRecord.Skip(1).Take(33).ToArray(); // Columns 2-34

                    while (csv.Read())
                    {
                        var record = new SurveyResponse
                        {
                            StudentId = csv.GetField(0) ?? "",
                            Saran = csv.GetField(34) ?? "",
                            KodeKelas = csv.GetField(35) ?? "",
                            NamaDosen = csv.GetField(36) ?? "",
                            MataKuliah = csv.GetField(37) ?? ""
                        };

                        for (int i = 1; i <= 33; i++)
                        {
                            record.Jawaban.Add(csv.GetField(i) ?? "");
                        }

                        responses.Add(record);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to read CSV {Path.GetFileName(filePath)}: {ex.Message}");
            }

            return responses;
        }

        public static List<LecturerReport> AggregateData(List<SurveyResponse> responses, string[] questionHeaders)
        {
            // Group by class to compute class averages
            var classGroups = responses
                .GroupBy(r => r.KodeKelas)
                .ToDictionary(
                    g => g.Key,
                    g => g.SelectMany(r => r.Jawaban.Select((j, i) => (Index: i, Score: ConvertToScore(j))))
                          .GroupBy(x => x.Index)
                          .Select(g => g.Average(x => x.Score))
                          .ToArray()
                );

            // Group by lecturer, course, class
            var grouped = responses
                .GroupBy(r => new { r.NamaDosen, r.MataKuliah, r.KodeKelas })
                .ToList();

            var reports = new List<LecturerReport>();

            foreach (var group in grouped)
            {
                var report = new LecturerReport
                {
                    NamaDosen = group.Key.NamaDosen,
                    MataKuliah = group.Key.MataKuliah,
                    KodeKelas = group.Key.KodeKelas,
                    Suggestions = group.Where(r => !string.IsNullOrWhiteSpace(r.Saran) && r.Saran != "-")
                                      .Select(r => r.Saran)
                                      .ToList()
                };

                // Calculate averages for each question
                var scores = new double[33];
                for (int i = 0; i < 33; i++)
                {
                    var questionScores = group.Select(r => ConvertToScore(r.Jawaban[i])).Where(s => s > 0).ToList();
                    scores[i] = questionScores.Any() ? questionScores.Average() : 0;
                }

                // Assign scores and class averages
                var classAverages = classGroups.ContainsKey(group.Key.KodeKelas) ? classGroups[group.Key.KodeKelas] : new double[33];
                for (int i = 0; i < questionHeaders.Length; i++)
                {
                    var cleanHeader = CleanQuestionHeader(questionHeaders[i]);
                    report.QuestionScores[cleanHeader] = (scores[i], classAverages.Length > i ? classAverages[i] : 0);
                }

                reports.Add(report);
            }

            return reports;
        }

        public static double ConvertToScore(string answer)
        {
            return answer.Trim() switch
            {
                "Sangat Setuju" => 4.0,
                "Setuju" => 3.0,
                "Tidak Setuju" => 2.0,
                "Sangat Tidak Setuju" => 1.0,
                _ => 0.0
            };
        }

        public static string CleanQuestionHeader(string header)
        {
            // Remove tokens and extract the core question
            var start = header.IndexOf('[');
            var end = header.IndexOf(']');
            if (start >= 0 && end > start)
            {
                return header.Substring(start + 1, end - start - 1);
            }
            return header;
        }
    }
}