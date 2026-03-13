using Microsoft.EntityFrameworkCore;
using school_management_system.Models;

namespace school_management_system.Services
{
    public class StudentPromotionService
    {
        private readonly MyDBContext _context;

        public StudentPromotionService(MyDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Promote students from one class to another based on exam results ranking.
        /// Assigns roll numbers for the target academic year in the target class.
        /// Students without results are appended after ranked students.
        /// </summary>
        public async Task PromoteClassByResultsAsync(int fromClassId, int toClassId, int examId, int targetAcademicYear)
        {
            // Load results for the exam and ensure exam belongs to fromClassId if possible
            var results = await _context.Results
                .Where(r => r.ExamID == examId)
                .Include(r => r.Student)
                .ToListAsync();

            // Filter students that are in fromClassId (current class)
            var filtered = results.Where(r => r.Student != null && r.Student.ClassID == fromClassId).ToList();

            // Ranked students: order by TotalMarks desc then by Percentage desc
            var ranked = filtered.OrderByDescending(r => r.TotalMarks).ThenByDescending(r => r.Percentage).ToList();

            int seq = 1;
            foreach (var r in ranked)
            {
                var s = r.Student!;

                s.ClassID = toClassId;
                s.AdmissionYear = targetAcademicYear;

                s.RollNumber = seq;

                _context.Students.Update(s);

                seq++;
            }

            // Now handle students in fromClassId who don't have results for this exam (append after ranked)
            var remaining = await _context.Students
                .Where(s => s.ClassID == fromClassId && !ranked.Any(r => r.StudentID == s.StudentID))
                .ToListAsync();
            foreach (var s in remaining)
            {
                s.ClassID = toClassId;
                s.AdmissionYear = targetAcademicYear;

                s.RollNumber = seq;

                _context.Students.Update(s);

                seq++;
            }

            await _context.SaveChangesAsync();
        }
    }
}
