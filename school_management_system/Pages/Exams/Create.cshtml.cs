using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using school_management_system.Models;

namespace school_management_system.Pages.Exams
{
    public class CreateModel : PageModel
    {
        private readonly MyDBContext _context;

        public CreateModel(MyDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Exam Exam { get; set; }

        public SelectList Classes { get; set; }

        public void OnGet()
        {
            Classes = new SelectList(_context.Classes, "ClassID", "ClassName");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Classes = new SelectList(_context.Classes, "ClassID", "ClassName", Exam.ClassID);
                return Page();
            }

            _context.Exams.Add(Exam);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
