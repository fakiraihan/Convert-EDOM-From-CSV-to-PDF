using System.Collections.Generic;

namespace EDOM
{
    public class LecturerReport
    {
        public string NamaDosen { get; set; }
        public string MataKuliah { get; set; }
        public string KodeKelas { get; set; }
        public Dictionary<string, (double Score, double ClassAverage)> QuestionScores { get; set; } = new Dictionary<string, (double, double)>();
        public List<string> Suggestions { get; set; } = new List<string>();
    }
}