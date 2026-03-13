using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_management_system.Models;

namespace school_management_system.Controllers
{
    [Route("marks-entry")]
    [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireTeacher")]
    public class MarksEntryController : Controller
    {
        private readonly MyDBContext _context;

        public MarksEntryController(MyDBContext context)
        {
            _context = context;
        }

        // GET: marks-entry?examId=1&classId=1
        [HttpGet]
        public async Task<IActionResult> Index(int examId, int classId)
        {
            var students = await _context.Students.Where(s => s.ClassID == classId).ToListAsync();
            var subjects = await _context.ClassSubjects.Where(cs => cs.ClassID == classId).Include(cs => cs.Subject).Select(cs => cs.Subject).ToListAsync();

            ViewBag.Exam = await _context.Exams.FindAsync(examId);
            ViewBag.Students = students;
            ViewBag.Subjects = subjects;

            // load existing marks
            var marks = await _context.Marks.Where(m => m.ExamID == examId && m.SubjectID != 0).ToListAsync();
            ViewBag.ExistingMarks = marks;

            return View();
        }

        // POST: marks-entry/save
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromForm] List<int> studentIds, [FromForm] List<int> subjectIds, [FromForm] List<int> marks, [FromForm] int examId)
        {
            // expects parallel lists studentIds, subjectIds, marks
            for (int i = 0; i < studentIds.Count; i++)
            {
                var sId = studentIds[i];
                var subId = subjectIds[i];
                var val = marks[i];

                var existing = await _context.Marks.FirstOrDefaultAsync(m => m.ExamID == examId && m.StudentID == sId && m.SubjectID == subId);
                if (existing != null)
                {
                    existing.Marks = val;
                    existing.IsPassed = val >= (_context.Subjects.Find(subId)?.PassMarks ?? 0);
                    _context.Marks.Update(existing);
                }
                else
                {
                    var m = new Mark { StudentID = sId, SubjectID = subId, ExamID = examId, Marks = val, IsPassed = val >= (_context.Subjects.Find(subId)?.PassMarks ?? 0) };
                    _context.Marks.Add(m);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { examId = examId, classId = _context.Exams.Find(examId)?.ClassID });
        }

        // Simple CSV upload for bulk marks (studentId,subjectId,marks)
        [HttpPost("upload-csv")] 
        public async Task<IActionResult> UploadCsv([FromForm] IFormFile file, [FromForm] int examId)
        {
            if (file == null || file.Length == 0) return BadRequest("File missing");

            using var sr = new StreamReader(file.OpenReadStream());
            while(!sr.EndOfStream)
            {
                var line = await sr.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                if (parts.Length < 3) continue;
                if (!int.TryParse(parts[0], out var sid)) continue;
                if (!int.TryParse(parts[1], out var subid)) continue;
                if (!int.TryParse(parts[2], out var val)) continue;

                var existing = await _context.Marks.FirstOrDefaultAsync(m => m.ExamID == examId && m.StudentID == sid && m.SubjectID == subid);
                if (existing != null)
                {
                    existing.Marks = val;
                    existing.IsPassed = val >= (_context.Subjects.Find(subid)?.PassMarks ?? 0);
                    _context.Marks.Update(existing);
                }
                else
                {
                    var m = new Mark { StudentID = sid, SubjectID = subid, ExamID = examId, Marks = val, IsPassed = val >= (_context.Subjects.Find(subid)?.PassMarks ?? 0) };
                    _context.Marks.Add(m);
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
