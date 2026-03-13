using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_management_system.Models
{
    public class Mark
    {
        [Key]
        public int MarkID { get; set; }

        [Required]
        public int StudentID { get; set; }

        [Required]
        public int SubjectID { get; set; }

        [Required]
        public int ExamID { get; set; }

        [Required]
        [Range(0, 100)]
        public int Marks { get; set; }

        public bool IsPassed { get; set; }

        // Navigation properties
        [ForeignKey(nameof(StudentID))]
        public Student? Student { get; set; }

        [ForeignKey(nameof(SubjectID))]
        public Subject? Subject { get; set; }

        [ForeignKey(nameof(ExamID))]
        public Exam? Exam { get; set; }
    }
}