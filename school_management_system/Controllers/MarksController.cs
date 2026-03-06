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
    public class MarksController : Controller
    {
        private readonly MyDBContext _context;

        public MarksController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Marks
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.Marks.Include(m => m.Exam).Include(m => m.Student).Include(m => m.Subject);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Marks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks
                .Include(m => m.Exam)
                .Include(m => m.Student)
                .Include(m => m.Subject)
                .FirstOrDefaultAsync(m => m.MarkID == id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // GET: Marks/Create
        public IActionResult Create()
        {
            ViewData["ExamID"] = new SelectList(_context.Exams, "ExamID", "ExamID");
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID");
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName");
            return View();
        }

        // POST: Marks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MarkID,StudentID,SubjectID,ExamID,Marks")] Mark mark)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExamID"] = new SelectList(_context.Exams, "ExamID", "ExamID", mark.ExamID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", mark.StudentID);
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName", mark.SubjectID);
            return View(mark);
        }

        // GET: Marks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks.FindAsync(id);
            if (mark == null)
            {
                return NotFound();
            }
            ViewData["ExamID"] = new SelectList(_context.Exams, "ExamID", "ExamID", mark.ExamID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", mark.StudentID);
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName", mark.SubjectID);
            return View(mark);
        }

        // POST: Marks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MarkID,StudentID,SubjectID,ExamID,Marks")] Mark mark)
        {
            if (id != mark.MarkID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarkExists(mark.MarkID))
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
            ViewData["ExamID"] = new SelectList(_context.Exams, "ExamID", "ExamID", mark.ExamID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", mark.StudentID);
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName", mark.SubjectID);
            return View(mark);
        }

        // GET: Marks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks
                .Include(m => m.Exam)
                .Include(m => m.Student)
                .Include(m => m.Subject)
                .FirstOrDefaultAsync(m => m.MarkID == id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // POST: Marks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mark = await _context.Marks.FindAsync(id);
            if (mark != null)
            {
                _context.Marks.Remove(mark);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarkExists(int id)
        {
            return _context.Marks.Any(e => e.MarkID == id);
        }
    }
}
