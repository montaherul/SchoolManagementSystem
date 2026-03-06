using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_management_system.Models
{
    public class FeePayment
    {
        [Key]
        public int PaymentID { get; set; }

        public int StudentID { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
       

        public DateTime PaymentDate { get; set; }

        public string? Method { get; set; }

        [ForeignKey("StudentID")]
        public Student? Student { get; set; }
    }
}