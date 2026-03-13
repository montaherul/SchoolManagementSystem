using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using school_management_system.Models;

namespace school_management_system.Pages.ExamRoutines
{
    public class IndexModel : PageModel
    {
        private readonly MyDBContext _context;

        public IndexModel(MyDBContext context)
        {
            _context = context;
        }

        public IList<ExamRoutine> Routines { get; set; }

        public async Task OnGetAsync()
        {
            Routines = await _context.ExamRoutines.Include(r => r.Exam).Include(r => r.Subject).Include(r => r.Invigilator).ToListAsync();
        }
    }
}
