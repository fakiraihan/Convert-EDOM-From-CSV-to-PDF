using System.Collections.Generic;

namespace EDOM
{
    public class SurveyResponse
    {
        public string StudentId { get; set; }
        public List<string> Jawaban { get; set; } = new List<string>();
        public string Saran { get; set; }
        public string KodeKelas { get; set; }
        public string NamaDosen { get; set; }
        public string MataKuliah { get; set; }
    }
}