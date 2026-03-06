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
    public class TeacherSalariesController : Controller
    {
        private readonly MyDBContext _context;

        public TeacherSalariesController(MyDBContext context)
        {
            _context = context;
        }

        // GET: TeacherSalaries
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.TeacherSalaries.Include(t => t.Teacher);
            return View(await myDBContext.ToListAsync());
        }

        // GET: TeacherSalaries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSalary = await _context.TeacherSalaries
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(m => m.SalaryID == id);
            if (teacherSalary == null)
            {
                return NotFound();
            }

            return View(teacherSalary);
        }

        // GET: TeacherSalaries/Create
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

        // POST: TeacherSalaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalaryID,TeacherID,BasicSalary,Allowance,Deduction,NetSalary")] TeacherSalary teacherSalary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherSalary);
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
      teacherSalary.TeacherID
  );
            return View(teacherSalary);
        }

        // GET: TeacherSalaries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSalary = await _context.TeacherSalaries.FindAsync(id);
            if (teacherSalary == null)
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
            return View(teacherSalary);
        }

        // POST: TeacherSalaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalaryID,TeacherID,BasicSalary,Allowance,Deduction,NetSalary")] TeacherSalary teacherSalary)
        {
            if (id != teacherSalary.SalaryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherSalary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherSalaryExists(teacherSalary.SalaryID))
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
      teacherSalary.TeacherID
  );
            return View(teacherSalary);
        }

        // GET: TeacherSalaries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSalary = await _context.TeacherSalaries
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(m => m.SalaryID == id);
            if (teacherSalary == null)
            {
                return NotFound();
            }

            return View(teacherSalary);
        }

        // POST: TeacherSalaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacherSalary = await _context.TeacherSalaries.FindAsync(id);
            if (teacherSalary != null)
            {
                _context.TeacherSalaries.Remove(teacherSalary);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherSalaryExists(int id)
        {
            return _context.TeacherSalaries.Any(e => e.SalaryID == id);
        }
    }
}
