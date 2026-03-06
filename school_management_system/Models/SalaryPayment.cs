using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_management_system.Models
{
    public class SalaryPayment
    {
        [Key]
        public int PaymentID { get; set; }

        public int TeacherID { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public string? Month { get; set; }

        public int Year { get; set; }

        public DateTime PaymentDate { get; set; }

        public string? Status { get; set; }

        [ForeignKey("TeacherID")]
        public Teacher? Teacher { get; set; }
    }
}