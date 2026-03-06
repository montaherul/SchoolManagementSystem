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
    public class TeacherSubjectsController : Controller
    {
        private readonly MyDBContext _context;

        public TeacherSubjectsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: TeacherSubjects
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.TeacherSubjects.Include(t => t.Class).Include(t => t.Section).Include(t => t.Subject).Include(t => t.Teacher);
            return View(await myDBContext.ToListAsync());
        }

        // GET: TeacherSubjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSubject = await _context.TeacherSubjects
                .Include(t => t.Class)
                .Include(t => t.Section)
                .Include(t => t.Subject)
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(m => m.TeacherSubjectID == id);
            if (teacherSubject == null)
            {
                return NotFound();
            }

            return View(teacherSubject);
        }

        // GET: TeacherSubjects/Create
        public IActionResult Create()
        {
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName");
            ViewData["SectionID"] = new SelectList(_context.Sections, "SectionID", "SectionName");
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName");
            ViewData["TeacherID"] = new SelectList(
     _context.Teachers.Select(t => new
     {
         t.TeacherID,
         Name = t.TeacherID + " - " + t.FirstName + " " + t.LastName
     }),
     "TeacherID",
     "Name"
 );
            return View();
        }

        // POST: TeacherSubjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherSubjectID,TeacherID,SubjectID,ClassID,SectionID")] TeacherSubject teacherSubject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherSubject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", teacherSubject.ClassID);
            ViewData["SectionID"] = new SelectList(_context.Sections, "SectionID", "SectionName", teacherSubject.SectionID);
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName", teacherSubject.SubjectID);
            ViewData["TeacherID"] = new SelectList(_context.Teachers.Select(t => new
                                                                                     {
                                                                                         t.TeacherID,
                                                                                         Name = t.TeacherID + " - " + t.FirstName + " " + t.LastName
                                                                                     }),
                                                                                     "TeacherID",
                                                                                     "Name"
                                                                                 );
            return View(teacherSubject);
        }

        // GET: TeacherSubjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSubject = await _context.TeacherSubjects.FindAsync(id);
            if (teacherSubject == null)
            {
                return NotFound();
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", teacherSubject.ClassID);
            ViewData["SectionID"] = new SelectList(_context.Sections, "SectionID", "SectionName", teacherSubject.SectionID);
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName", teacherSubject.SubjectID);
            ViewData["TeacherID"] = new SelectList(
    _context.Teachers.Select(t => new
    {
        t.TeacherID,
        Name = t.TeacherID + " - " + t.FirstName + " " + t.LastName
    }),
    "TeacherID",
    "Name"
);
            return View(teacherSubject);
        }

        // POST: TeacherSubjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherSubjectID,TeacherID,SubjectID,ClassID,SectionID")] TeacherSubject teacherSubject)
        {
            if (id != teacherSubject.TeacherSubjectID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherSubjectExists(teacherSubject.TeacherSubjectID))
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
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", teacherSubject.ClassID);
            ViewData["SectionID"] = new SelectList(_context.Sections, "SectionID", "SectionName", teacherSubject.SectionID);
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectName", teacherSubject.SubjectID);
            ViewData["TeacherID"] = new SelectList(
     _context.Teachers.Select(t => new
     {
         t.TeacherID,
         Name = t.TeacherID + " - " + t.FirstName + " " + t.LastName
     }),
     "TeacherID",
     "Name"
 );
            return View(teacherSubject);
        }

        // GET: TeacherSubjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSubject = await _context.TeacherSubjects
                .Include(t => t.Class)
                .Include(t => t.Section)
                .Include(t => t.Subject)
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(m => m.TeacherSubjectID == id);
            if (teacherSubject == null)
            {
                return NotFound();
            }

            return View(teacherSubject);
        }

        // POST: TeacherSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacherSubject = await _context.TeacherSubjects.FindAsync(id);
            if (teacherSubject != null)
            {
                _context.TeacherSubjects.Remove(teacherSubject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherSubjectExists(int id)
        {
            return _context.TeacherSubjects.Any(e => e.TeacherSubjectID == id);
        }
    }
}
