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

        // Payroll Report
        public IActionResult PayrollReport()
        {
            var payroll = _context.SalaryPayments
                .Include(p => p.Teacher)
                .ToList();

            return View(payroll);
        }
        public IActionResult StudentReportPDF()
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