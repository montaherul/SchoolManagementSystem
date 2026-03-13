using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace school_management_system.Models
{
    public class Exam
    {
        [Key]
        public int ExamID { get; set; }

        public string? ExamName { get; set; } // Mid, Final

        public int ClassID { get; set; }

        [ForeignKey("ClassID")]
        public Class? Class { get; set; }

        public DateTime ExamDate { get; set; }

        public ICollection<Mark>? Marks { get; set; }
    }
}
