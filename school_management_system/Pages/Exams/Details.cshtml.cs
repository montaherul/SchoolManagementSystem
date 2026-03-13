using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using school_management_system.Models;

namespace school_management_system.Pages.Exams
{
    public class DetailsModel : PageModel
    {
        private readonly MyDBContext _context;

        public DetailsModel(MyDBContext context)
        {
            _context = context;
        }

        public Exam Exam { get; set; }

        public async Task OnGetAsync(int id)
        {
            Exam = await _context.Exams.Include(e => e.Class).Include(e => e.Marks).FirstOrDefaultAsync(e => e.ExamID == id);
        }
    }
}
