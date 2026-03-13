using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_management_system.Models;

namespace school_management_system.Pages.Exams
{
    public class EditModel : PageModel
    {
        private readonly MyDBContext _context;

        public EditModel(MyDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Exam Exam { get; set; }

        public SelectList Classes { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Exam = await _context.Exams.FindAsync(id);
            if (Exam == null) return NotFound();
            Classes = new SelectList(_context.Classes, "ClassID", "ClassName", Exam.ClassID);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Classes = new SelectList(_context.Classes, "ClassID", "ClassName", Exam.ClassID);
                return Page();
            }

            _context.Attach(Exam).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
