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
    public class FeePaymentsController : Controller
    {
        private readonly MyDBContext _context;

        public FeePaymentsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: FeePayments
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.FeePayments.Include(f => f.Student);
            return View(await myDBContext.ToListAsync());
        }

        // GET: FeePayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feePayment = await _context.FeePayments
                .Include(f => f.Student)
                .FirstOrDefaultAsync(m => m.PaymentID == id);
            if (feePayment == null)
            {
                return NotFound();
            }

            return View(feePayment);
        }

        // GET: FeePayments/Create
        public IActionResult Create()
        {
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID");
            return View();
        }

        // POST: FeePayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentID,StudentID,Amount,PaymentDate,Method")] FeePayment feePayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feePayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", feePayment.StudentID);
            return View(feePayment);
        }

        // GET: FeePayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feePayment = await _context.FeePayments.FindAsync(id);
            if (feePayment == null)
            {
                return NotFound();
            }
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", feePayment.StudentID);
            return View(feePayment);
        }

        // POST: FeePayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentID,StudentID,Amount,PaymentDate,Method")] FeePayment feePayment)
        {
            if (id != feePayment.PaymentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feePayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeePaymentExists(feePayment.PaymentID))
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
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", feePayment.StudentID);
            return View(feePayment);
        }

        // GET: FeePayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feePayment = await _context.FeePayments
                .Include(f => f.Student)
                .FirstOrDefaultAsync(m => m.PaymentID == id);
            if (feePayment == null)
            {
                return NotFound();
            }

            return View(feePayment);
        }

        // POST: FeePayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feePayment = await _context.FeePayments.FindAsync(id);
            if (feePayment != null)
            {
                _context.FeePayments.Remove(feePayment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeePaymentExists(int id)
        {
            return _context.FeePayments.Any(e => e.PaymentID == id);
        }
    }
}
