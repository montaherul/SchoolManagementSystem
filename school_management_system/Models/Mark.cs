using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_management_system.Models
{
    public class Mark
    {
        [Key]
        public int MarkID { get; set; }

        public int StudentID { get; set; }

        public int SubjectID { get; set; }

        public int ExamID { get; set; }

        public int Marks { get; set; }

        [ForeignKey("StudentID")]
        public Student? Student { get; set; }

        [ForeignKey("SubjectID")]
        public Subject? Subject { get; set; }

        [ForeignKey("ExamID")]
        public Exam? Exam { get; set; }
    }
}