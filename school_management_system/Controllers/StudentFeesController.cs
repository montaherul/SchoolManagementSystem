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
    public class StudentFeesController : Controller
    {
        private readonly MyDBContext _context;

        public StudentFeesController(MyDBContext context)
        {
            _context = context;
        }

        // GET: StudentFees
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.StudentFees.Include(s => s.FeeType).Include(s => s.Student);
            return View(await myDBContext.ToListAsync());
        }

        // GET: StudentFees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentFee = await _context.StudentFees
                .Include(s => s.FeeType)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.StudentFeeID == id);
            if (studentFee == null)
            {
                return NotFound();
            }

            return View(studentFee);
        }

        // GET: StudentFees/Create
        public IActionResult Create()
        {
            ViewData["FeeTypeID"] = new SelectList(_context.FeeTypes, "FeeTypeID", "FeeTypeID");
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID");
            return View();
        }

        // POST: StudentFees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentFeeID,StudentID,FeeTypeID,Amount,DueDate,Status")] StudentFee studentFee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentFee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FeeTypeID"] = new SelectList(_context.FeeTypes, "FeeTypeID", "FeeTypeID", studentFee.FeeTypeID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", studentFee.StudentID);
            return View(studentFee);
        }

        // GET: StudentFees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentFee = await _context.StudentFees.FindAsync(id);
            if (studentFee == null)
            {
                return NotFound();
            }
            ViewData["FeeTypeID"] = new SelectList(_context.FeeTypes, "FeeTypeID", "FeeTypeID", studentFee.FeeTypeID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", studentFee.StudentID);
            return View(studentFee);
        }

        // POST: StudentFees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentFeeID,StudentID,FeeTypeID,Amount,DueDate,Status")] StudentFee studentFee)
        {
            if (id != studentFee.StudentFeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentFee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentFeeExists(studentFee.StudentFeeID))
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
            ViewData["FeeTypeID"] = new SelectList(_context.FeeTypes, "FeeTypeID", "FeeTypeID", studentFee.FeeTypeID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", studentFee.StudentID);
            return View(studentFee);
        }

        // GET: StudentFees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentFee = await _context.StudentFees
                .Include(s => s.FeeType)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.StudentFeeID == id);
            if (studentFee == null)
            {
                return NotFound();
            }

            return View(studentFee);
        }

        // POST: StudentFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentFee = await _context.StudentFees.FindAsync(id);
            if (studentFee != null)
            {
                _context.StudentFees.Remove(studentFee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentFeeExists(int id)
        {
            return _context.StudentFees.Any(e => e.StudentFeeID == id);
        }
    }
}
