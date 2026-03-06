using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_management_system.Models
{
    public class Exam
    {
        [Key]
        public int ExamID { get; set; }

        public string? ExamName { get; set; }

        public int ClassID { get; set; }

        [ForeignKey("ClassID")]
        public Class? Class { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExamDate { get; set; }
    }
}