using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_management_system.Models
{
    public class StudentFee
    {
        [Key]
        public int StudentFeeID { get; set; }

        public int StudentID { get; set; }

        public int FeeTypeID { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
     

        public DateTime DueDate { get; set; }

        public string? Status { get; set; }

        [ForeignKey("StudentID")]
        public Student? Student { get; set; }

        [ForeignKey("FeeTypeID")]
        public FeeType? FeeType { get; set; }
    }
}