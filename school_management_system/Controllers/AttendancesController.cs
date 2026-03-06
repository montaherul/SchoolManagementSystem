using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_management_system;
using school_management_system.Models;
using school_management_system.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace school_management_system.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly MyDBContext _context;

        public AttendancesController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Attendances
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.Attendances.Include(a => a.Student);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Attendances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AttendanceID == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendances/Create
        public IActionResult Create()
        {
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID");
            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttendanceID,StudentID,Date,Status,Method")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", attendance.StudentID);
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", attendance.StudentID);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttendanceID,StudentID,Date,Status,Method")] Attendance attendance)
        {
            if (id != attendance.AttendanceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.AttendanceID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", attendance.StudentID);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AttendanceID == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(e => e.AttendanceID == id);
        }


        // Mark Attendance Page
        public async Task<IActionResult> MarkAttendance()
        {
            var students = await _context.Students.ToListAsync();
            return View(students);
        }
        [HttpPost]
        public async Task<IActionResult> MarkAttendance(List<Attendance> attendanceList)
        {
            foreach (var item in attendanceList)
            {
                item.Date = DateTime.Now;

                _context.Attendances.Add(item);

                var student = await _context.Students.FindAsync(item.StudentID);

                if (item.Status == "Absent")
                {
                    SMSService sms = new SMSService();

                    string message =
                    $"Dear Parent, {student.FirstName} {student.LastName} is ABSENT today.";

                    sms.SendSMS(student.ParentPhone, message);

                    SMSLog log = new SMSLog
                    {
                        StudentID = student.StudentID,
                        Phone = student.ParentPhone,
                        Message = message,
                        SentDate = DateTime.Now,
                        Status = "Sent"
                    };

                    _context.SMSLogs.Add(log);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("AttendanceDashboard");
        }
        public IActionResult AttendanceDashboard()
        {
            var attendance = _context.Attendances
                .Include(a => a.Student)
                .OrderByDescending(a => a.Date)
                .ToList();

            return View(attendance);
        }


        // Monthly Attendance
        public async Task<IActionResult> MonthlyAttendance()
        {
            var data = await _context.Attendances
                .Include(a => a.Student)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            return View(data);
        }
    }
}
