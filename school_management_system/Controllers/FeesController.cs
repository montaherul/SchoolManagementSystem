using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_management_system.Models;

namespace school_management_system.Controllers
{
    public class FeesController : Controller
    {
        private readonly MyDBContext _context;

        public FeesController(MyDBContext context)
        {
            _context = context;
        }

        // Fee Collection Page
        public async Task<IActionResult> FeeCollection()
        {
            var students = await _context.Students.ToListAsync();
            return View(students);
        }

        // Payment History
        public async Task<IActionResult> PaymentHistory()
        {
            var payments = await _context.FeePayments
                .Include(p => p.Student)
                .ToListAsync();

            return View(payments);
        }

        // Student Fee Details
        public async Task<IActionResult> StudentFeeDetails(int id)
        {
            var fees = await _context.StudentFees
                .Include(f => f.Student)
                .Include(f => f.FeeType)
                .Where(f => f.StudentID == id)
                .ToListAsync();

            return View(fees);
        }
        public IActionResult FeesDashboard()
        {
            ViewBag.TotalStudents = _context.Students.Count();
            ViewBag.TotalFees = _context.StudentFees.Sum(f => f.Amount);
            ViewBag.TotalPayments = _context.FeePayments.Sum(p => p.Amount);

            return View();
        }
    }
}