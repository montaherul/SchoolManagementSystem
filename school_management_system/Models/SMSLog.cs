using System;
using System.ComponentModel.DataAnnotations;

namespace school_management_system.Models
{
    public class SMSLog
    {
        [Key]
        public int SMSID { get; set; }

        public int StudentID { get; set; }

        public string? Phone { get; set; }

        public string? Message { get; set; }

        public DateTime SentDate { get; set; }

        public string? Status { get; set; }

        public Student? Student { get; set; }
    }
}