using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using school_management_system.Models;
using ClosedXML.Excel;
using System.IO;

namespace school_management_system.Pages.MarksEntry
{
    [Microsoft.AspNetCore.Authorization.Authorize(Policy = "RequireTeacher")]
    public class IndexModel : PageModel
    {
        private readonly MyDBContext _context;

        public IndexModel(MyDBContext context)
        {
            _context = context;
        }

        // expose simple lists for selection in page
        public IEnumerable<Exam> Exams => _context.Exams.ToList();
        public IEnumerable<Class> Classes => _context.Classes.ToList();

        [BindProperty(SupportsGet = true)]
        public int ExamId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int ClassId { get; set; }

        public Exam? Exam { get; set; }
        public List<Student> Students { get; set; } = new();
        public List<Subject> Subjects { get; set; } = new();
        public List<Mark> ExistingMarks { get; set; } = new();
        public int RowsProcessed { get; set; }
        public List<string> ImportErrors { get; set; } = new();

        // Bind marks arrays on POST
        [BindProperty]
        public List<int> studentIds { get; set; } = new();
        [BindProperty]
        public List<int> subjectIds { get; set; } = new();
        [BindProperty]
        public List<int> marks { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (ExamId != 0)
            {
                Exam = await _context.Exams.FindAsync(ExamId);
            }

            if (ClassId == 0 && Exam != null)
            {
                ClassId = Exam.ClassID;
            }

            if (ClassId != 0)
            {
                Students = await _context.Students.Where(s => s.ClassID == ClassId).ToListAsync();
                Subjects = await _context.ClassSubjects.Where(cs => cs.ClassID == ClassId).Include(cs => cs.Subject).Select(cs => cs.Subject).ToListAsync();
            }

            if (ExamId != 0)
            {
                ExistingMarks = await _context.Marks.Where(m => m.ExamID == ExamId).ToListAsync();
            }

            // provide lists to view via ViewData for the select lists
            ViewData["Exams"] = await _context.Exams.ToListAsync();
            ViewData["Classes"] = await _context.Classes.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (studentIds == null || subjectIds == null || marks == null)
            {
                ModelState.AddModelError(string.Empty, "No marks submitted.");
                await OnGetAsync();
                return Page();
            }

            for (int i = 0; i < studentIds.Count; i++)
            {
                var sId = studentIds[i];
                var subId = subjectIds[i];
                var val = marks[i];

                var existing = await _context.Marks.FirstOrDefaultAsync(m => m.ExamID == ExamId && m.StudentID == sId && m.SubjectID == subId);
                if (existing != null)
                {
                    existing.Marks = val;
                    existing.IsPassed = val >= (_context.Subjects.Find(subId)?.PassMarks ?? 0);
                    _context.Marks.Update(existing);
                }
                else
                {
                    var m = new Mark { StudentID = sId, SubjectID = subId, ExamID = ExamId, Marks = val, IsPassed = val >= (_context.Subjects.Find(subId)?.PassMarks ?? 0) };
                    _context.Marks.Add(m);
                }
            }

            await _context.SaveChangesAsync();

            // redirect back to same page to show updated marks
            return RedirectToPage(new { ExamId = ExamId, ClassId = ClassId });
        }

        public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "File missing");
                await OnGetAsync();
                return Page();
            }

            using var sr = new StreamReader(file.OpenReadStream());
            while (!sr.EndOfStream)
            {
                var line = await sr.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                if (parts.Length < 3) continue;

                string studentKey = parts[0].Trim();
                int sid = 0;

                // Try parse as StudentID first
                if (int.TryParse(studentKey, out var parsedId))
                {
                    // First try StudentID
                    var s = await _context.Students.FindAsync(parsedId);

                    if (s != null)
                    {
                        sid = s.StudentID;
                    }
                    else
                    {
                        // If not StudentID, try RollNumber
                        var s2 = await _context.Students
                            .FirstOrDefaultAsync(st => st.RollNumber == parsedId);

                        if (s2 != null)
                            sid = s2.StudentID;
                    }
                }
                else
                {
                    // If studentKey is not a number
                    ImportErrors.Add($"Student not found for '{studentKey}'");
                }
                if (sid == 0)
                {
                    ImportErrors.Add($"CSV: student not found for '{studentKey}'");
                    continue;
                }

                if (!int.TryParse(parts[1], out var subid))
                {
                    ImportErrors.Add($"CSV: invalid subject id '{parts[1]}'");
                    continue;
                }
                if (!int.TryParse(parts[2], out var val))
                {
                    ImportErrors.Add($"CSV: invalid marks '{parts[2]}'");
                    continue;
                }

                var existing = await _context.Marks.FirstOrDefaultAsync(m => m.ExamID == ExamId && m.StudentID == sid && m.SubjectID == subid);
                if (existing != null)
                {
                    existing.Marks = val;
                    existing.IsPassed = val >= (_context.Subjects.Find(subid)?.PassMarks ?? 0);
                    _context.Marks.Update(existing);
                }
                else
                {
                    var m = new Mark { StudentID = sid, SubjectID = subid, ExamID = ExamId, Marks = val, IsPassed = val >= (_context.Subjects.Find(subid)?.PassMarks ?? 0) };
                    _context.Marks.Add(m);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage(new { ExamId = ExamId, ClassId = ClassId });
        }

        public async Task<IActionResult> OnPostUploadExcelAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "File missing");
                await OnGetAsync();
                return Page();
            }

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;

            using var wb = new XLWorkbook(ms);
            var ws = wb.Worksheets.First();

            // Expect header row with StudentID,SubjectID,Marks or columns in that order
            var firstRow = 1;
            for (int row = firstRow + 1; row <= ws.LastRowUsed().RowNumber(); row++)
            {
                var sidCell = ws.Cell(row, 1).GetValue<string>();
                var subCell = ws.Cell(row, 2).GetValue<string>();
                var marksCell = ws.Cell(row, 3).GetValue<string>();

                int sid;
                if (!int.TryParse(sidCell, out sid))
                {
                    if (int.TryParse(sidCell, out int roll))
                    {
                        var student = await _context.Students
                            .FirstOrDefaultAsync(st => st.RollNumber == roll);

                        if (student != null)
                        {
                            sid = student.StudentID;
                        }
                        else
                        {
                            ImportErrors.Add($"Row {row}: student not found for '{sidCell}'");
                            continue;
                        }
                    }
                    else
                    {
                        ImportErrors.Add($"Row {row}: invalid student value '{sidCell}'");
                        continue;
                    }
                }
                if (!int.TryParse(subCell, out var subid)) { ImportErrors.Add($"Row {row}: invalid subject id '{subCell}'"); continue; }
                if (!int.TryParse(marksCell, out var val)) { ImportErrors.Add($"Row {row}: invalid marks '{marksCell}'"); continue; }

                var existing = await _context.Marks.FirstOrDefaultAsync(m => m.ExamID == ExamId && m.StudentID == sid && m.SubjectID == subid);
                if (existing != null)
                {
                    existing.Marks = val;
                    existing.IsPassed = val >= (_context.Subjects.Find(subid)?.PassMarks ?? 0);
                    _context.Marks.Update(existing);
                }
                else
                {
                    var m = new Mark { StudentID = sid, SubjectID = subid, ExamID = ExamId, Marks = val, IsPassed = val >= (_context.Subjects.Find(subid)?.PassMarks ?? 0) };
                    _context.Marks.Add(m);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage(new { ExamId = ExamId, ClassId = ClassId });
        }
    }
}
