using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using school_management_system.Models;

namespace school_management_system.Pages.ExamRoutines
{
    public class DeleteModel : PageModel
    {
        private readonly MyDBContext _context;

        public DeleteModel(MyDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ExamRoutine Routine { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Routine = await _context.ExamRoutines.Include(r => r.Exam).Include(r => r.Subject).FirstOrDefaultAsync(r => r.ExamRoutineID == id);
            if (Routine == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var r = await _context.ExamRoutines.FindAsync(id);
            if (r != null) _context.ExamRoutines.Remove(r);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
