using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_management_system.Models;

namespace school_management_system.Pages.ExamRoutines
{
    public class CreateModel : PageModel
    {
        private readonly MyDBContext _context;

        public CreateModel(MyDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ExamRoutine Routine { get; set; }

        public SelectList Exams { get; set; }
        public SelectList Subjects { get; set; }
        public SelectList Teachers { get; set; }

        public void OnGet()
        {
            Exams = new SelectList(_context.Exams, "ExamID", "ExamName");
            Subjects = new SelectList(_context.Subjects, "SubjectID", "SubjectName");
            Teachers = new SelectList(_context.Teachers, "TeacherID", "TeacherName");
        }

        // Simple conflict detection: same room or invigilator overlap
        private bool HasConflict(ExamRoutine r)
        {
            var conflicts = _context.ExamRoutines.Where(x => x.Date.Date == r.Date.Date && x.ExamID == r.ExamID && x.ExamRoutineID != r.ExamRoutineID).ToList();

            foreach(var c in conflicts)
            {
                // invigilator conflict
                if (r.InvigilatorID.HasValue && c.InvigilatorID == r.InvigilatorID)
                {
                    if (!(r.EndTime <= c.StartTime || r.StartTime >= c.EndTime)) return true;
                }
                // room conflict
                if (!string.IsNullOrEmpty(r.Room) && !string.IsNullOrEmpty(c.Room) && r.Room == c.Room)
                {
                    if (!(r.EndTime <= c.StartTime || r.StartTime >= c.EndTime)) return true;
                }
            }
            return false;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                OnGet();
                return Page();
            }

            if (HasConflict(Routine))
            {
                ModelState.AddModelError(string.Empty, "Routine conflict detected for the selected invigilator or room/time.");
                OnGet();
                return Page();
            }

            _context.ExamRoutines.Add(Routine);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
