using school_management_system.Models;
using Microsoft.EntityFrameworkCore;

namespace school_management_system.Services
{
    public class ResultCalculator
    {
        private readonly MyDBContext _context;
        private readonly IConfiguration _config;

        public ResultCalculator(MyDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Calculate results for a given exam and class
        public async Task GenerateResultsForExamAsync(int examId)
        {
            var exam = await _context.Exams.FindAsync(examId);
            if (exam == null) return;

            var students = await _context.Students.Where(s => s.ClassID == exam.ClassID).ToListAsync();
            var subjects = await _context.ClassSubjects.Where(cs => cs.ClassID == exam.ClassID).Include(cs => cs.Subject).Select(cs => cs.Subject).ToListAsync();

            var results = new List<Result>();

            foreach (var s in students)
            {
                var marks = await _context.Marks.Where(m => m.ExamID == examId && m.StudentID == s.StudentID).Include(m => m.Subject).ToListAsync();
                if (!marks.Any()) continue; // skip students without marks

                int totalObtained = marks.Sum(m => m.Marks);
                int totalMax = marks.Sum(m => m.Subject?.TotalMarks ?? 100);
                double percentage = totalMax == 0 ? 0 : (double)totalObtained / totalMax * 100.0;

                var gpa = CalculateGPA(marks);
                var grade = GradeFromPercentage(percentage);
                bool passed = marks.All(m => m.IsPassed);

                var res = new Result
                {
                    StudentID = s.StudentID,
                    ExamID = examId,
                    TotalMarks = totalObtained,
                    Percentage = percentage,
                    GPA = gpa,
                    Grade = grade,
                    Position = 0 // compute later
                };

                results.Add(res);
            }

            // compute positions
            var ordered = results.OrderByDescending(r => r.TotalMarks).ToList();
            int pos = 1;
            for (int i = 0; i < ordered.Count; i++)
            {
                if (i > 0 && ordered[i].TotalMarks == ordered[i-1].TotalMarks)
                {
                    ordered[i].Position = ordered[i-1].Position; // tie handling
                }
                else
                {
                    ordered[i].Position = pos;
                }
                pos++;
            }

            // persist results (replace existing for exam)
            var existing = _context.Results.Where(r => r.ExamID == examId);
            _context.Results.RemoveRange(existing);
            _context.Results.AddRange(ordered);
            await _context.SaveChangesAsync();
        }

        private double CalculateGPA(IEnumerable<Mark> marks)
        {
            // Simple GPA: map mark percentage to 5.0 scale per subject and average
            var scores = new List<double>();
            foreach (var m in marks)
            {
                int total = m.Subject?.TotalMarks ?? 100;
                double pct = total == 0 ? 0 : (double)m.Marks / total * 100.0;
                scores.Add(GradePointFromPercentage(pct));
            }
            return scores.Count == 0 ? 0 : scores.Average();
        }

        private double GradePointFromPercentage(double pct)
        {
            // Allow configuration via appsettings: Grades:APercent, APercent, BPercent, CPercent
            double aPlus = GetConfigDouble("Grades:APercent", 80);
            double a = GetConfigDouble("Grades:APercent", 70);
            double aMinus = GetConfigDouble("Grades:AMinusPercent", 60);
            double b = GetConfigDouble("Grades:BPercent", 50);
            double c = GetConfigDouble("Grades:CPercent", 40);

            if (pct >= aPlus) return 5.0;
            if (pct >= a) return 4.0;
            if (pct >= aMinus) return 3.5;
            if (pct >= b) return 3.0;
            if (pct >= c) return 2.0;
            return 0.0;
        }

        private string GradeFromPercentage(double pct)
        {
            double aPlus = GetConfigDouble("Grades:APercent", 80);
            double a = GetConfigDouble("Grades:APercent", 70);
            double aMinus = GetConfigDouble("Grades:AMinusPercent", 60);
            double b = GetConfigDouble("Grades:BPercent", 50);
            double c = GetConfigDouble("Grades:CPercent", 40);

            if (pct >= aPlus) return "A+";
            if (pct >= a) return "A";
            if (pct >= aMinus) return "A-";
            if (pct >= b) return "B";
            if (pct >= c) return "C";
            return "F";
        }

        private double GetConfigDouble(string key, double defaultValue)
        {
            if (_config == null) return defaultValue;
            var s = _config[key];
            if (double.TryParse(s, out var v)) return v;
            return defaultValue;
        }
    }
}
