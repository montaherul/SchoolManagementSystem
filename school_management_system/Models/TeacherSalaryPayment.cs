using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_management_system.Models
{
    public class TeacherSalaryPayment
    {
        [Key]
        public int PaymentID { get; set; }

        public int TeacherID { get; set; }

        public string? Month { get; set; }

        public int Year { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }

        public DateTime PaymentDate { get; set; }

        public string? PaymentMethod { get; set; }

        public Teacher? Teacher { get; set; }
    }
}