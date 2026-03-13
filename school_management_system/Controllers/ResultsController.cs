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
    [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireAdmin")]
    public class ResultsController : Controller
    {
        private readonly MyDBContext _context;
        private readonly school_management_system.Services.ResultCalculator _calculator;

        public ResultsController(MyDBContext context, school_management_system.Services.ResultCalculator calculator)
        {
            _context = context;
            _calculator = calculator;
        }

        // GET: Results
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.Results.Include(r => r.Exam).Include(r => r.Student);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Results/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _context.Results
                .Include(r => r.Exam)
                .Include(r => r.Student)
                .FirstOrDefaultAsync(m => m.ResultID == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // GET: Results/Create
        public IActionResult Create()
        {
            ViewData["ExamID"] = new SelectList(_context.Exams, "ExamID", "ExamID");
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID");
            return View();
        }

        // POST: Results/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResultID,StudentID,ExamID,TotalMarks,Percentage,Grade")] Result result)
        {
            if (ModelState.IsValid)
            {
                _context.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExamID"] = new SelectList(_context.Exams, "ExamID", "ExamID", result.ExamID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", result.StudentID);
            return View(result);
        }

        // GET: Results/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _context.Results.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            ViewData["ExamID"] = new SelectList(_context.Exams, "ExamID", "ExamID", result.ExamID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", result.StudentID);
            return View(result);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResultID,StudentID,ExamID,TotalMarks,Percentage,Grade")] Result result)
        {
            if (id != result.ResultID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultExists(result.ResultID))
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
            ViewData["ExamID"] = new SelectList(_context.Exams, "ExamID", "ExamID", result.ExamID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", result.StudentID);
            return View(result);
        }

        // GET: Results/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _context.Results
                .Include(r => r.Exam)
                .Include(r => r.Student)
                .FirstOrDefaultAsync(m => m.ResultID == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _context.Results.FindAsync(id);
            if (result != null)
            {
                _context.Results.Remove(result);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResultExists(int id)
        {
            return _context.Results.Any(e => e.ResultID == id);
        }

        // POST: Results/Generate/5
        [HttpPost]
        public async Task<IActionResult> Generate(int id)
        {
            await _calculator.GenerateResultsForExamAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Results/Publish/5
        [HttpPost]
        public async Task<IActionResult> Publish(int id)
        {
            var list = _context.Results.Where(r => r.ExamID == id);
            foreach(var r in list)
            {
                r.IsPublished = true;
                r.PublishedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Results/Lock/5
        [HttpPost]
        public async Task<IActionResult> Lock(int id)
        {
            var list = _context.Results.Where(r => r.ExamID == id);
            foreach(var r in list)
            {
                r.IsLocked = true;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
