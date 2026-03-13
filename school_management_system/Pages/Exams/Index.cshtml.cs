using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using school_management_system.Models;

namespace school_management_system.Pages.Exams
{
    public class IndexModel : PageModel
    {
        private readonly MyDBContext _context;

        public IndexModel(MyDBContext context)
        {
            _context = context;
        }

        public IList<Exam> ExamList { get; set; }

        public async Task OnGetAsync()
        {
            ExamList = await _context.Exams.Include(e => e.Class).ToListAsync();
        }
    }
}
