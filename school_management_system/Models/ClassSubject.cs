using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_management_system.Models
{
    public class ClassSubject
    {
        [Key]
        public int ID { get; set; }

        // Foreign Keys
        public int ClassID { get; set; }

        public int SubjectID { get; set; }

        [ForeignKey("ClassID")]
        public Class Class { get; set; }

        [ForeignKey("SubjectID")]
        public Subject Subject { get; set; }
    }
}