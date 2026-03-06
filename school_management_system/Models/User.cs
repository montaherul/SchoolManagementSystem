using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public int RoleID { get; set; }

        public virtual Role Role { get; set; }
    }
}