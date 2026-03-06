using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_management_system.Models
{
    public class Section
    {
        [Key]
        public int SectionID { get; set; }

        [Required]
        public string SectionName { get; set; }

        // Foreign Key
        public int ClassID { get; set; }

        [ForeignKey("ClassID")]
        public Class Class { get; set; }
    }
}