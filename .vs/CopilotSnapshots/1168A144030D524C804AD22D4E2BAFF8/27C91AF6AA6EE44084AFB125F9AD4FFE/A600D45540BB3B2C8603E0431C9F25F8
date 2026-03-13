using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using SchoolManagementSystem.Models;
using school_management_system;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyDBContext _context;

        public AccountController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please enter username and password";
                return View();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserID", user.UserID.ToString());
                HttpContext.Session.SetString("Username", user.Username ?? "");
                HttpContext.Session.SetString("Role", user.Role?.RoleName ?? "");

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid Username or Password";
            return View();
        }

        // GET: Register
        public IActionResult Register()
        {
            ViewBag.RoleID = new SelectList(_context.Roles, "RoleID", "RoleName");
            return View();
        }

        // POST: Register
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
           // if (ModelState.IsValid)
           // {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
           // }

          //  ViewBag.RoleID = new SelectList(_context.Roles, "RoleID", "RoleName", user.RoleID);
            // return View(user);
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}