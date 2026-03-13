using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_management_system;
using school_management_system.Models;
using System.IO;

namespace school_management_system.Controllers
{
    public class StudentsController : Controller
    {
        private readonly MyDBContext _context;

        public StudentsController(MyDBContext context)
        {
            _context = context;
        }

        // =========================
        // STUDENT LIST
        // =========================

        public async Task<IActionResult> Index()
        {
            var students = _context.Students
                .Include(s => s.Class)
                .Include(s => s.Section);

            return View(await students.ToListAsync());
        }

        // =========================
        // STUDENT DETAILS
        // =========================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Section)
                .FirstOrDefaultAsync(m => m.StudentID == id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // =========================
        // CREATE STUDENT (GET)
        // =========================

        public IActionResult Create()
        {
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName");

            ViewData["SectionID"] = new SelectList(new List<Section>(), "SectionID", "SectionName");

            return View();
        }

        // =========================
        // CREATE STUDENT (POST)
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                if (student.Photo != null)
                {
                    string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/students");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid().ToString() +
                                      Path.GetExtension(student.Photo.FileName);

                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await student.Photo.CopyToAsync(stream);
                    }

                    student.PhotoPath = "/images/students/" + fileName;
                }

                _context.Add(student);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", student.ClassID);
            ViewData["SectionID"] = new SelectList(_context.Sections, "SectionID", "SectionName", student.SectionID);

            return View(student);
        }

        // =========================
        // EDIT STUDENT (GET)
        // =========================

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound();

            // Load classes
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", student.ClassID);

            // Load sections based on selected class
            var sections = _context.Sections
                .Where(s => s.ClassID == student.ClassID)
                .ToList();

            ViewData["SectionID"] = new SelectList(sections, "SectionID", "SectionName", student.SectionID);

            return View(student);
        }
        // =========================
        // EDIT STUDENT (POST)
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.StudentID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingStudent = await _context.Students
                        .AsNoTracking()
                        .FirstOrDefaultAsync(s => s.StudentID == id);

                    if (student.Photo != null)
                    {
                        string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/students");

                        string fileName = Guid.NewGuid().ToString() +
                                          Path.GetExtension(student.Photo.FileName);

                        string filePath = Path.Combine(folder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await student.Photo.CopyToAsync(stream);
                        }

                        student.PhotoPath = "/images/students/" + fileName;
                    }
                    else
                    {
                        student.PhotoPath = existingStudent.PhotoPath;
                    }

                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentID))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", student.ClassID);

            ViewData["SectionID"] = new SelectList(_context.Sections, "SectionID", "SectionName", student.SectionID);

            return View(student);
        }

        // =========================
        // DELETE STUDENT (GET)
        // =========================

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Section)
                .FirstOrDefaultAsync(m => m.StudentID == id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // =========================
        // DELETE STUDENT (POST)
        // =========================

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student != null)
            {
                if (!string.IsNullOrEmpty(student.PhotoPath))
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(),
                                               "wwwroot",
                                               student.PhotoPath.TrimStart('/'));

                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }

                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // PROFILE PAGE
        // =========================

        public async Task<IActionResult> Profile(int id)
        {
            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Section)
                .FirstOrDefaultAsync(s => s.StudentID == id);

            if (student == null)
                return NotFound();

            // Exams for student's class
            var exams = await _context.Exams
                .Where(e => e.ClassID == student.ClassID)
                .ToListAsync();

            // Results for this student
            // Build detailed exam-wise results with subject marks
            var examResults = new List<school_management_system.Models.StudentExamResult>();

            var studentResults = await _context.Results
                .Where(r => r.StudentID == id)
                .Include(r => r.Exam)
                .ToListAsync();

            foreach (var res in studentResults)
            {
                var marks = await _context.Marks
                    .Where(m => m.StudentID == id && m.ExamID == res.ExamID)
                    .Include(m => m.Subject)
                    .ToListAsync();

                var ser = new school_management_system.Models.StudentExamResult
                {
                    Exam = res.Exam,
                    Marks = marks,
                    TotalMarks = res.TotalMarks,
                    Percentage = res.Percentage,
                    GPA = res.GPA,
                    Grade = res.Grade,
                    Position = res.Position,
                    IsPublished = res.IsPublished
                };

                examResults.Add(ser);
            }

            ViewBag.Exams = exams;
            ViewBag.Results = examResults;

            return View(student);
        }

        // =========================
        // CHECK EXIST
        // =========================

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }
        //GET SECTION 


        [HttpGet]
        public async Task<JsonResult> GetSections(int classId)
        {
            var sections = await _context.Sections
                .Where(s => s.ClassID == classId)
                .Select(s => new
                {
                    sectionID = s.SectionID,
                    sectionName = s.SectionName
                })
                .ToListAsync();

            return Json(sections);
        }
    }
}