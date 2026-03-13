using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using school_management_system.Models;

namespace school_management_system.Pages.Exams
{
    public class DeleteModel : PageModel
    {
        private readonly MyDBContext _context;

        public DeleteModel(MyDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Exam Exam { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Exam = await _context.Exams.Include(e => e.Class).FirstOrDefaultAsync(e => e.ExamID == id);
            if (Exam == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam != null) _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
