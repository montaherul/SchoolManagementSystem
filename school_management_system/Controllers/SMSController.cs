using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_management_system.Models;

namespace school_management_system.Controllers
{
    public class SMSController : Controller
    {
        private readonly MyDBContext _context;

        public SMSController(MyDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SendSMS()
        {
            ViewBag.Students = _context.Students.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult SendSMS(int StudentID, string Message)
        {
            var student = _context.Students.Find(StudentID);

            SMSLog log = new SMSLog
            {
                StudentID = StudentID,
                Phone = student.ParentPhone,
                Message = Message,
                SentDate = DateTime.Now,
                Status = "Sent"
            };

            _context.SMSLogs.Add(log);
            _context.SaveChanges();

            return RedirectToAction("SMSHistory");
        }

        public IActionResult SMSHistory()
        {
            var logs = _context.SMSLogs
                .Include(s => s.Student)
                .ToList();

            return View(logs);
        }
    }
}