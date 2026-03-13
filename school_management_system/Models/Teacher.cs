using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_management_system.Models
{
    public class Teacher
    {
        public int TeacherID { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? PhotoPath { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }

        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }
    }
}