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
    public class ClassSubjectsController : Controller
    {
        private readonly MyDBContext _context;

        public ClassSubjectsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: ClassSubjects
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.ClassSubjects.Include(c => c.Class).Include(c => c.Subject);
            return View(await myDBContext.ToListAsync());
        }

        // GET: ClassSubjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classSubject = await _context.ClassSubjects
                .Include(c => c.Class)
                .Include(c => c.Subject)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (classSubject == null)
            {
                return NotFound();
            }

            return View(classSubject);
        }

        // GET: ClassSubjects/Create
        public IActionResult Create()
        {
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName");
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName");
            return View();
        }

        // POST: ClassSubjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ClassID,SubjectID")] ClassSubject classSubject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classSubject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", classSubject.ClassID);
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName", classSubject.SubjectID);
            return View(classSubject);
        }

        // GET: ClassSubjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classSubject = await _context.ClassSubjects.FindAsync(id);
            if (classSubject == null)
            {
                return NotFound();
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", classSubject.ClassID);
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName", classSubject.SubjectID);
            return View(classSubject);
        }

        // POST: ClassSubjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ClassID,SubjectID")] ClassSubject classSubject)
        {
            if (id != classSubject.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassSubjectExists(classSubject.ID))
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
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", classSubject.ClassID);
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName", classSubject.SubjectID);
            return View(classSubject);
        }

        // GET: ClassSubjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classSubject = await _context.ClassSubjects
                .Include(c => c.Class)
                .Include(c => c.Subject)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (classSubject == null)
            {
                return NotFound();
            }

            return View(classSubject);
        }

        // POST: ClassSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classSubject = await _context.ClassSubjects.FindAsync(id);
            if (classSubject != null)
            {
                _context.ClassSubjects.Remove(classSubject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassSubjectExists(int id)
        {
            return _context.ClassSubjects.Any(e => e.ID == id);
        }
    }
}
