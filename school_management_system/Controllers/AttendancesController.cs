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
        private readonly NotificationService _notification;

        public AttendancesController(MyDBContext context, NotificationService notification)
        {
            _context = context;
            _notification = notification;
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
            if (id == null) return NotFound();

            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AttendanceID == id);

            if (attendance == null) return NotFound();

            return View(attendance);
        }

        // GET: Attendances/Create
        public IActionResult Create()
        {
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID");
            return View();
        }

        // POST: Attendances/Create
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
            if (id == null) return NotFound();

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null) return NotFound();

            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", attendance.StudentID);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttendanceID,StudentID,Date,Status,Method")] Attendance attendance)
        {
            if (id != attendance.AttendanceID) return NotFound();

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
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", attendance.StudentID);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AttendanceID == id);

            if (attendance == null) return NotFound();

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

        // ===============================
        // MARK ATTENDANCE MODULE
        // ===============================

        public async Task<IActionResult> MarkAttendance(int? classId, int? sectionId)
        {
            // Load classes
            ViewBag.Classes = await _context.Classes.ToListAsync();

            // Load sections based on selected class
            if (classId != null)
            {
                ViewBag.Sections = await _context.Sections
                    .Where(s => s.ClassID == classId)
                    .ToListAsync();
            }
            else
            {
                ViewBag.Sections = new List<Section>();
            }

            // Load students based on class + section
            List<Student> students = new List<Student>();

            if (classId.HasValue && sectionId.HasValue)
            {
                students = await _context.Students
                    .Where(s => s.ClassID == classId.Value && s.SectionID == sectionId.Value)
                    .ToListAsync();
            }

            return View(students);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAttendance(List<Attendance> attendanceList)
        {
            var today = DateTime.Today;

            foreach (var item in attendanceList)
            {
                // Skip if status not selected
                if (string.IsNullOrEmpty(item.Status))
                    continue;

                var existingAttendance = await _context.Attendances
                    .FirstOrDefaultAsync(a =>
                        a.StudentID == item.StudentID &&
                        a.Date == today);

                if (existingAttendance == null)
                {
                    // Insert new attendance
                    item.Date = today;
                    item.Method = "Manual";

                    _context.Attendances.Add(item);
                }
                else
                {
                    // Update existing attendance
                    existingAttendance.Status = item.Status;
                    existingAttendance.Method = "Manual";
                }

                var student = await _context.Students.FindAsync(item.StudentID);

                // Send SMS only once if absent
                if (student != null && item.Status == "Absent")
                {
                    bool smsAlreadySent = await _context.SMSLogs
                        .AnyAsync(s =>
                            s.StudentID == student.StudentID &&
                            s.SentDate.Date == today &&
                            s.Message.Contains("ABSENT"));

                    if (!smsAlreadySent)
                    {
                        string message =
                            $"Dear {student.ParentName}, {student.FirstName} {student.LastName} is ABSENT today.";

                        try
                        {
                            _notification.SendNotification(
                                student.ParentPhone,
                                student.ParentEmail,
                                message
                            );
                        }
                        catch
                        {
                            // ignore notification errors
                        }

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
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("AttendanceDashboard");
        }
        // ===============================
        // DASHBOARD
        // ===============================

        public IActionResult AttendanceDashboard()
        {
            var today = DateTime.Today;

            var totalStudents = _context.Students.Count();

            var todayAttendance = _context.Attendances
                .Where(a => a.Date.Date == today)
                .ToList();

            var present = todayAttendance.Count(a => a.Status == "Present");
            var absent = todayAttendance.Count(a => a.Status == "Absent");
            var late = todayAttendance.Count(a => a.Status == "Late");

            ViewBag.TotalStudents = totalStudents;
            ViewBag.Present = present;
            ViewBag.Absent = absent;
            ViewBag.Late = late;

            var attendance = _context.Attendances
                .Include(a => a.Student)
                .OrderByDescending(a => a.Date)
                .Take(50)
                .ToList();

            return View(attendance);
        }
        // ===============================
        // MONTHLY ATTENDANCE
        // ===============================

        public async Task<IActionResult> MonthlyAttendance(int? month, int? year, int? classId, int? sectionId)
        {
            if (month == null) month = DateTime.Now.Month;
            if (year == null) year = DateTime.Now.Year;

            ViewBag.Month = month;
            ViewBag.Year = year;

            ViewBag.Classes = await _context.Classes.ToListAsync();

            if (classId != null)
            {
                ViewBag.Sections = await _context.Sections
                    .Where(s => s.ClassID == classId)
                    .ToListAsync();
            }
            else
            {
                ViewBag.Sections = new List<Section>();
            }

            var query = _context.Attendances
                .Include(a => a.Student)
                .AsQueryable();

            query = query.Where(a => a.Date.Month == month && a.Date.Year == year);

            if (classId != null)
                query = query.Where(a => a.Student.ClassID == classId);

            if (sectionId != null)
                query = query.Where(a => a.Student.SectionID == sectionId);

            var data = await query
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            return View(data);
        }

        // ===============================
        // STUDENT ATTENDANCE
        // ===============================

        public async Task<IActionResult> StudentAttendance(int id)
        {
            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .Where(a => a.StudentID == id)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            int total = attendance.Count;
            int present = attendance.Count(a => a.Status == "Present");
            int absent = attendance.Count(a => a.Status == "Absent");
          

            ViewBag.AttendancePercent = total == 0 ? 0 :
                (present * 100) / total;
            ViewBag.Totalpresentday= present;
            ViewBag.Totalabsentday= absent;

            return View(attendance);
        }
        //get section
        [HttpGet]
        public async Task<JsonResult> GetSections(int classId)
        {
            var sections = await _context.Sections
                .Where(s => s.ClassID == classId)
                .Select(s => new
                {
                    sectionID = s.SectionID,
                    sectionName = s.SectionName
                })
                .ToListAsync();

            return Json(sections);
        }

    }
}