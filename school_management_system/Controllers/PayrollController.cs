using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using school_management_system.Models;
using Rotativa.AspNetCore;


namespace school_management_system.Controllers
{
    public class PayrollController : Controller
    {
        private readonly MyDBContext _context;

        public PayrollController(MyDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        // Salary Structure
        public async Task<IActionResult> SalaryStructure()
        {
            var salaries = await _context.SalaryStructures
                .Include(s => s.Teacher)
                .ToListAsync();

            return View(salaries);
        }

        // Pay Salary
        public IActionResult PaySalary()
        {
            ViewBag.Teachers = _context.Teachers.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PaySalary(SalaryPayment payment)
        {
            payment.PaymentDate = DateTime.Now;

            _context.SalaryPayments.Add(payment);

            await _context.SaveChangesAsync();

            return RedirectToAction("SalaryHistory");
        }

        // Salary History
        public async Task<IActionResult> SalaryHistory()
        {
            var history = await _context.SalaryPayments
                .Include(p => p.Teacher)
                .ToListAsync();

            return View(history);
        }


      
        public IActionResult SalarySlip(int id)
    {
        var salary = _context.SalaryPayments
            .Include(s => s.Teacher)
            .FirstOrDefault(s => s.PaymentID == id);

        return new ViewAsPdf("SalarySlip", salary)
        {
            FileName = "SalarySlip.pdf"
        };
    }

}
}