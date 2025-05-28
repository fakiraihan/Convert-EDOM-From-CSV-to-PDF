using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections.Generic;
using System.Linq;

namespace EDOM
{
    public class PdfGenerator
    {
        private static readonly (string Category, int[] Indices)[] Categories = new[]
        {
            ("Kompetensi Pedagogik", Enumerable.Range(0, 13).ToArray()),
            ("Kompetensi Profesional", Enumerable.Range(13, 5).ToArray()),
            ("Kepribadian Dosen", Enumerable.Range(18, 5).ToArray()),
            ("Metode Pembelajaran", Enumerable.Range(23, 5).ToArray()),
            ("Media Pembelajaran", Enumerable.Range(28, 5).ToArray())
        };

        public static void GeneratePdf(List<LecturerReport> reports, string outputPath)
        {
            using (var writer = new PdfWriter(outputPath))
            using (var pdf = new PdfDocument(writer))
            using (var document = new Document(pdf))
            {
                document.SetMargins(36, 36, 36, 36);

                foreach (var report in reports)
                {
                    // Header
                    document.Add(new Paragraph("Evaluasi Dosen oleh Taruna")
                        .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER));
                    document.Add(new Paragraph("Politeknik Siber dan Sandi Negara | Jurusan Kriptografi")
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.CENTER));
                    document.Add(new Paragraph("Semester Gasal T.A. 2024/2025")
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.CENTER));
                    document.Add(new Paragraph($"Nama Dosen: {report.NamaDosen}")
                        .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                        .SetFontSize(12));
                    document.Add(new Paragraph($"Mata Kuliah: {report.MataKuliah} ({report.KodeKelas})")
                        .SetFontSize(12));
                    document.Add(new Paragraph("\n"));

                    // Table for scores
                    foreach (var (category, indices) in Categories)
                    {
                        document.Add(new Paragraph(category)
                            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                            .SetFontSize(14));

                        var table = new Table(UnitValue.CreatePercentArray(new float[] { 70, 15, 15 }))
                            .UseAllAvailableWidth();
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Aspek/Pertanyaan")
                            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Nilai")
                            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Rata-Rata Kelas")
                            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))));

                        var questions = report.QuestionScores.ToList();
                        foreach (var index in indices)
                        {
                            if (index < questions.Count)
                            {
                                var (question, (score, classAvg)) = questions[index];
                                table.AddCell(new Cell().Add(new Paragraph(question)));
                                table.AddCell(new Cell().Add(new Paragraph(score.ToString("0.00"))));
                                table.AddCell(new Cell().Add(new Paragraph(classAvg.ToString("0.00"))));
                            }
                        }

                        document.Add(table);
                        document.Add(new Paragraph("\n"));
                    }

                    // Suggestions
                    if (report.Suggestions.Any())
                    {
                        document.Add(new Paragraph("Saran/Masukan:")
                            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                            .SetFontSize(12));
                        foreach (var saran in report.Suggestions)
                        {
                            document.Add(new Paragraph($"- {saran}"));
                        }
                    }

                    document.Add(new Paragraph("Keterangan: Penilaian dalam skala 4.")
                        .SetFontSize(10)
                        .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE)));
                    if (report != reports.Last())
                    {
                        document.Add(new AreaBreak());
                    }
                }
            }
        }
    }
}