using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickLearnLMS.Data;
using QuickLearnLMS.Models;

namespace QuickLearnLMS.Controllers
{
    public class AdminController : Controller
    {
        private readonly LMSContext _context;

        public AdminController(LMSContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View(); // Views/Admin/Dashboard.cshtml
        }

        [HttpGet]
        public IActionResult AddUser() => View();

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        public IActionResult AddCourse()
        {
            // For dropdown of teachers
            ViewBag.Teachers = _context.Users
                .Where(u => u.Role == "Teacher")
                .ToList();

            return View();
        }

        [HttpPost]
        public IActionResult AddCourse(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        public IActionResult AssignCourse()
        {
            ViewBag.Courses = _context.Courses.Include(c => c.Teacher).ToList();
            ViewBag.Teachers = _context.Users.Where(u => u.Role == "Teacher").ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AssignCourse(int courseId, int teacherId)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseID == courseId);
            if (course != null)
            {
                course.TeacherID = teacherId;
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }
        public IActionResult ManageUsers()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _context.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            try
            {
                _context.Users.Update(user);
                _context.SaveChanges();
                return RedirectToAction("ManageUsers");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.InnerException?.Message ?? ex.Message;
                return View(user);
            }
        }


        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("ManageUsers");
        }
        public IActionResult ManageCourses()
        {
            var courses = _context.Courses.Include(c => c.Teacher).ToList();
            return View(courses);
        }

        [HttpGet]
        public IActionResult EditCourse(int id)
        {
            var course = _context.Courses.Find(id);
            ViewBag.Teachers = _context.Users.Where(u => u.Role == "Teacher").ToList();
            return View(course);
        }

        [HttpPost]
        public IActionResult EditCourse(Course course)
        {
            _context.Courses.Update(course);
            _context.SaveChanges();
            return RedirectToAction("ManageCourses");
        }

        public IActionResult DeleteCourse(int id)
        {
            var course = _context.Courses.Find(id);
            _context.Courses.Remove(course);
            _context.SaveChanges();
            return RedirectToAction("ManageCourses");
        }

    }
}
