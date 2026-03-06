using System.ComponentModel.DataAnnotations;
using static System.Collections.Specialized.BitVector32;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace school_management_system.Models
{
    public class Class
    {
        [Key]
        public int ClassID { get; set; }

        [Required]
        public string ClassName { get; set; }

        // Navigation properties
        public ICollection<Section> Sections { get; set; }

        public ICollection<ClassSubject> ClassSubjects { get; set; }
    }
}