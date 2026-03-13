using System.Collections.Generic;

namespace school_management_system.Models
{
    public class StudentExamResult
    {
        public Exam? Exam { get; set; }
        public List<Mark> Marks { get; set; } = new List<Mark>();
        public int TotalMarks { get; set; }
        public double Percentage { get; set; }
        public double GPA { get; set; }
        public string? Grade { get; set; }
        public int Position { get; set; }
        public bool IsPublished { get; set; }
    }
}
