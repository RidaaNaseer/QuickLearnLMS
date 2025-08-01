using Microsoft.AspNetCore.Mvc;
using QuickLearnLMS.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;

public class AccountController : Controller
{
    private readonly LMSContext _context;

    public AccountController(LMSContext context)
    {
        _context = context;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string email, string password)
    
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        if (user != null)
        {
            // Store data in session
            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetString("Role", user.Role);

            if (user.Role == "Admin")
                return RedirectToAction("Dashboard", "Admin");
            else if (user.Role == "Teacher")
                return RedirectToAction("Dashboard", "Teacher");
            else
                return RedirectToAction("Dashboard", "Student");
        }

        ViewBag.Error = "Invalid credentials";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
