using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_management_system.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceID { get; set; }

        public int StudentID { get; set; }

        [ForeignKey("StudentID")]
        public Student? Student { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string? Status { get; set; }

        public string? Method { get; set; }
    }
}