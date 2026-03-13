using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace school_management_system.Models
{
    public class Result
    {
        [Key]
        public int ResultID { get; set; }

        public int StudentID { get; set; }

        public int ExamID { get; set; }

        public int TotalMarks { get; set; }

        public double Percentage { get; set; }

        public double GPA { get; set; }

        public string? Grade { get; set; }

        public int Position { get; set; }

        // Publication/locking
        public bool IsPublished { get; set; }
        public bool IsLocked { get; set; }

        public DateTime? PublishedAt { get; set; }

        [ForeignKey("StudentID")]
        public Student? Student { get; set; }

        [ForeignKey("ExamID")]
        public Exam? Exam { get; set; }
    }
}
