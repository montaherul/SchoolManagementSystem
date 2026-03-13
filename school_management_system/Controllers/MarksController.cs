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

        // =========================
        // MARKS LIST
        // =========================
        public async Task<IActionResult> Index()
        {
            var marks = _context.Marks
                .Include(m => m.Student)
                .Include(m => m.Subject)
                .Include(m => m.Exam);

            return View(await marks.ToListAsync());
        }

        // =========================
        // DETAILS
        // =========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var mark = await _context.Marks
                .Include(m => m.Student)
                .Include(m => m.Subject)
                .Include(m => m.Exam)
                .FirstOrDefaultAsync(m => m.MarkID == id);

            if (mark == null)
                return NotFound();

            return View(mark);
        }

        // =========================
        // CREATE (GET)
        // =========================
        public IActionResult Create()
        {
            ViewData["ExamID"] = new SelectList(_context.Exams, "ExamID", "ExamName");

            ViewData["StudentID"] = new SelectList(
                _context.Students.Select(s => new
                {
                    s.StudentID,
                    Name = s.RollNumber + " - " + s.FirstName + " " + s.LastName
                }),
                "StudentID",
                "Name"
            );

            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName");

            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MarkID,StudentID,SubjectID,ExamID,Marks")] Mark mark)
        {
            var exists = await _context.Marks.AnyAsync(m =>
                m.StudentID == mark.StudentID &&
                m.SubjectID == mark.SubjectID &&
                m.ExamID == mark.ExamID);

            if (exists)
            {
                ModelState.AddModelError("", "Marks already entered for this student in this subject and exam.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(mark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ExamID"] = new SelectList(_context.Exams, "ExamID", "ExamName", mark.ExamID);

            ViewData["StudentID"] = new SelectList(
                _context.Students.Select(s => new
                {
                    s.StudentID,
                    Name = s.RollNumber + " - " + s.FirstName + " " + s.LastName
                }),
                "StudentID",
                "Name",
                mark.StudentID
            );

            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName", mark.SubjectID);

            return View(mark);
        }

        // =========================
        // EDIT (GET)
        // =========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var mark = await _context.Marks.FindAsync(id);

            if (mark == null)
                return NotFound();

            ViewData["ExamID"] = new SelectList(_context.Exams, "ExamID", "ExamName", mark.ExamID);

            ViewData["StudentID"] = new SelectList(
                _context.Students.Select(s => new
                {
                    s.StudentID,
                    Name = s.RollNumber + " - " + s.FirstName   + " " + s.LastName
                }),
                "StudentID",
                "Name",
                mark.StudentID
            );

            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName", mark.SubjectID);

            return View(mark);
        }

        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MarkID,StudentID,SubjectID,ExamID,Marks")] Mark mark)
        {
            if (id != mark.MarkID)
                return NotFound();

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
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ExamID"] = new SelectList(_context.Exams, "ExamID", "ExamName", mark.ExamID);

            ViewData["StudentID"] = new SelectList(
                _context.Students.Select(s => new
                {
                    s.StudentID,
                    Name = s.RollNumber + " - " + s.FirstName    + " " + s.LastName
                }),
                "StudentID",
                "Name",
                mark.StudentID
            );

            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName", mark.SubjectID);

            return View(mark);
        }

        // =========================
        // DELETE
        // =========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var mark = await _context.Marks
                .Include(m => m.Student)
                .Include(m => m.Subject)
                .Include(m => m.Exam)
                .FirstOrDefaultAsync(m => m.MarkID == id);

            if (mark == null)
                return NotFound();

            return View(mark);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mark = await _context.Marks.FindAsync(id);

            if (mark != null)
                _context.Marks.Remove(mark);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // CHECK EXIST
        // =========================
        private bool MarkExists(int id)
        {
            return _context.Marks.Any(e => e.MarkID == id);
        }
    }
}