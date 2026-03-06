using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace school_management_system.Models
{
    public class Student
    {
        public int StudentID { get; set; }

        public string? AdmissionNumber { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string? Address { get; set; }

        public string? ParentName { get; set; }

        public string? ParentPhone { get; set; }

        public string? ParentEmail { get; set; }

        public DateTime AdmissionDate { get; set; }

        public int ClassID { get; set; }

        public int SectionID { get; set; }

        public string? Status { get; set; }

        public string? PhotoPath { get; set; }

        [NotMapped]
        public IFormFile? Photo { get; set; }

        // Navigation properties
        public Class? Class { get; set; }

        public Section? Section { get; set; }

    }
}