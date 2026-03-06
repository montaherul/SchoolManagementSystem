using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_management_system;
using school_management_system.Models;

namespace school_management_system.Controllers
{
    public class TeacherSalaryPaymentsController : Controller
    {
        private readonly MyDBContext _context;

        public TeacherSalaryPaymentsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: TeacherSalaryPayments
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.TeacherSalaryPayments.Include(t => t.Teacher);
            return View(await myDBContext.ToListAsync());
        }

        // GET: TeacherSalaryPayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSalaryPayment = await _context.TeacherSalaryPayments
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(m => m.PaymentID == id);
            if (teacherSalaryPayment == null)
            {
                return NotFound();
            }

            return View(teacherSalaryPayment);
        }

        // GET: TeacherSalaryPayments/Create
        public IActionResult Create()
        {
            ViewData["TeacherID"] = new SelectList(
    _context.Teachers.Select(t => new
    {
        t.TeacherID,
        Name = t.FirstName + " " + t.LastName
    }),
    "TeacherID",
    "Name"
);
            return View();
        }

        // POST: TeacherSalaryPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentID,TeacherID,Month,Year,AmountPaid,PaymentDate,PaymentMethod")] TeacherSalaryPayment teacherSalaryPayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherSalaryPayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherID"] = new SelectList(
      _context.Teachers.Select(t => new
      {
          t.TeacherID,
          Name = t.FirstName + " " + t.LastName
      }),
      "TeacherID",
      "Name",
      teacherSalaryPayment.TeacherID
  );
            return View(teacherSalaryPayment);
        }

        // GET: TeacherSalaryPayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSalaryPayment = await _context.TeacherSalaryPayments.FindAsync(id);
            if (teacherSalaryPayment == null)
            {
                return NotFound();
            }
            ViewData["TeacherID"] = new SelectList(
      _context.Teachers.Select(t => new
      {
          t.TeacherID,
          Name = t.FirstName + " " + t.LastName
      }),
      "TeacherID",
      "Name"
  );
            return View(teacherSalaryPayment);
        }

        // POST: TeacherSalaryPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentID,TeacherID,Month,Year,AmountPaid,PaymentDate,PaymentMethod")] TeacherSalaryPayment teacherSalaryPayment)
        {
            if (id != teacherSalaryPayment.PaymentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherSalaryPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherSalaryPaymentExists(teacherSalaryPayment.PaymentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherID"] = new SelectList(
         _context.Teachers.Select(t => new
         {
             t.TeacherID,
             Name = t.FirstName + " " + t.LastName
         }),
         "TeacherID",
         "Name",
         teacherSalaryPayment.TeacherID
     );
            return View(teacherSalaryPayment);
        }

        // GET: TeacherSalaryPayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSalaryPayment = await _context.TeacherSalaryPayments
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(m => m.PaymentID == id);
            if (teacherSalaryPayment == null)
            {
                return NotFound();
            }

            return View(teacherSalaryPayment);
        }

        // POST: TeacherSalaryPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacherSalaryPayment = await _context.TeacherSalaryPayments.FindAsync(id);
            if (teacherSalaryPayment != null)
            {
                _context.TeacherSalaryPayments.Remove(teacherSalaryPayment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherSalaryPaymentExists(int id)
        {
            return _context.TeacherSalaryPayments.Any(e => e.PaymentID == id);
        }
    }
}
