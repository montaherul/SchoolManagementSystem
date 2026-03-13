using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace school_management_system.Models
{
    public class Subject
    {
        [Key]
        public int SubjectID { get; set; }

        [Required]
        public string SubjectName { get; set; }
        public int TotalMarks { get; set; } = 100;

        public int PassMarks { get; set; } = 33;

        // Navigation property
        public ICollection<ClassSubject> ClassSubjects { get; set; }
    }
}