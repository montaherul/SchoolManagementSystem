using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace school_management_system.Models
{
    public class ExamRoutine
    {
        [Key]
        public int ExamRoutineID { get; set; }

        public int ExamID { get; set; }
        [ForeignKey("ExamID")]
        public Exam? Exam { get; set; }

        public int SubjectID { get; set; }
        [ForeignKey("SubjectID")]
        public Subject? Subject { get; set; }

        // Date of the exam for this subject
        public DateTime Date { get; set; }

        // Time window
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // Optional room and invigilator
        public string? Room { get; set; }

        public int? InvigilatorID { get; set; }
        [ForeignKey("InvigilatorID")]
        public Teacher? Invigilator { get; set; }
    }
}
