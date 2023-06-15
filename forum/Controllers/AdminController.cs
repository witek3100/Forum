using System.ComponentModel.DataAnnotations;
using Forum.Data;
using Forum.Models;
using Microsoft.AspNetCore.Mvc;

namespace forum.Controllers
{
    public class AdminController : Controller
    {
        private readonly ForumDbContext _context;

        public AdminController(ForumDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {   
            string? token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }

            var user = _context.User.FirstOrDefault(u => u.token == HttpContext.Session.GetString("token"));
            if (user == null)
            {
                return Problem("User does not exist.");
            }

            if (user.role != "admin")
            {
                return RedirectToAction("SignIn", "Auth");
            }

            return View();
        }

        
    }
}