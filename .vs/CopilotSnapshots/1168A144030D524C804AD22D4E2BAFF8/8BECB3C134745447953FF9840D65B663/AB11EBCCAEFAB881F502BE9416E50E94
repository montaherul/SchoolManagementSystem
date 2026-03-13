using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_management_system.Models;
using Rotativa.AspNetCore;
using ClosedXML.Excel;
using System.IO;

namespace school_management_system.Controllers
{
    public class ReportsController : Controller
    {
        private readonly MyDBContext _context;

        public ReportsController(MyDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        // Student Report
        public IActionResult StudentReport()
        {
            var students = _context.Students
                .Include(s => s.Class)
                .Include(s => s.Section)
                .ToList();

            return View(students);
        }

        // Attendance Report
        public IActionResult AttendanceReport()
        {
            var attendance = _context.Attendances
                .Include(a => a.Student)
                .ToList();

            return View(attendance);
        }

        // Fee Report
        public IActionResult FeeReport()
        {
            var fees = _context.FeePayments
                .Include(f => f.Student)
                .ToList();

            return View(fees);
        }

        // Result Report
        public IActionResult ResultReport()
        {
            var results = _context.Results
                .Include(r => r.Student)
                .Include(r => r.Exam)
                .ToList();

            return View(results);
        }

        // Export results for an exam as Excel
        public async Task<IActionResult> ExportResultsExcel(int examId)
        {
            var results = await _context.Results.Include(r => r.Student).Where(r => r.ExamID == examId).ToListAsync();
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Results");
            ws.Cell(1, 1).Value = "StudentID";
            ws.Cell(1, 2).Value = "StudentName";
            ws.Cell(1, 3).Value = "TotalMarks";
            ws.Cell(1, 4).Value = "Percentage";
            ws.Cell(1, 5).Value = "GPA";
            ws.Cell(1, 6).Value = "Grade";
            ws.Cell(1, 7).Value = "Position";

            int row = 2;
            foreach(var r in results)
            {
                ws.Cell(row, 1).Value = r.StudentID;
                ws.Cell(row, 2).Value = (r.Student?.FirstName + " " + r.Student?.LastName)?.Trim();
                ws.Cell(row, 3).Value = r.TotalMarks;
                ws.Cell(row, 4).Value = r.Percentage;
                ws.Cell(row, 5).Value = r.GPA;
                ws.Cell(row, 6).Value = r.Grade;
                ws.Cell(row, 7).Value = r.Position;
                row++;
            }

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"results_exam_{examId}.xlsx");
        }

        // Export marks for a class and exam in matrix format (students x subjects)
        public async Task<IActionResult> ExportMarksMatrix(int examId, int classId)
        {
            var students = await _context.Students.Where(s => s.ClassID == classId).ToListAsync();
            var subjects = await _context.ClassSubjects.Where(cs => cs.ClassID == classId).Include(cs => cs.Subject).Select(cs => cs.Subject).ToListAsync();

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Marks");

            ws.Cell(1, 1).Value = "StudentID";
            ws.Cell(1, 2).Value = "StudentName";
            for (int j = 0; j < subjects.Count; j++)
            {
                ws.Cell(1, 3 + j).Value = subjects[j].SubjectName;
            }

            int row = 2;
            foreach (var s in students)
            {
                ws.Cell(row, 1).Value = s.StudentID;
                ws.Cell(row, 2).Value = s.FirstName + " " + s.LastName;
                for (int j = 0; j < subjects.Count; j++)
                {
                    var sub = subjects[j];
                    var mark = await _context.Marks.FirstOrDefaultAsync(m => m.ExamID == examId && m.StudentID == s.StudentID && m.SubjectID == sub.SubjectID);
                    ws.Cell(row, 3 + j).Value = mark?.Marks ?? 0;
                }
                row++;
            }

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"marks_exam_{examId}_class_{classId}.xlsx");
        }

        // PDF report card per student using Rotativa (render Razor view to PDF)
        public async Task<IActionResult> StudentReportPdf(int examId, int studentId)
        {
            var result = await _context.Results.Include(r => r.Student).Include(r => r.Exam).FirstOrDefaultAsync(r => r.ExamID == examId && r.StudentID == studentId);
            if (result == null) return NotFound();

            return new ViewAsPdf("StudentReport", result)
            {
                FileName = $"report_student_{studentId}_exam_{examId}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }

        // Payroll Report
        public IActionResult PayrollReport()
        {
            var payroll = _context.SalaryPayments
                .Include(p => p.Teacher)
                .ToList();

            return View(payroll);
        }
        public IActionResult StudentReportBulkPdf()
        {
            var students = _context.Students
                .Include(s => s.Class)
                .Include(s => s.Section)
                .Include(s=> s.Photo)
                .ToList();

            return new ViewAsPdf("StudentReport", students)
            {
                FileName = "StudentReport.pdf"
            };
        }
        public IActionResult StudentReportExcel()
        {
            var students = _context.Students.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Students");

                worksheet.Cell(1, 1).Value = "Name";
                worksheet.Cell(1, 2).Value = "Parent";


                int row = 2;

                foreach (var s in students)
                {
                    worksheet.Cell(row, 1).Value = s.FirstName + " " + s.LastName;
                    worksheet.Cell(row, 2).Value = s.ParentName;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);

                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "StudentReport.xlsx");
                }
            }
        }
    }
}