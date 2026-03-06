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

        // Navigation property
        public ICollection<ClassSubject> ClassSubjects { get; set; }
    }
}