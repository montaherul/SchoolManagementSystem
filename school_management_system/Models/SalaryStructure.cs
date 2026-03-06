using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_management_system.Models
{
    public class SalaryStructure
    {
        [Key]
        public int SalaryID { get; set; }

        public int TeacherID { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicSalary { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Allowance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Bonus { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Deduction { get; set; }

        [ForeignKey("TeacherID")]
        public Teacher? Teacher { get; set; }
    }
}