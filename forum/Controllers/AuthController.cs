using System.ComponentModel.DataAnnotations;
using Forum.Data;
using Forum.Models;
using Microsoft.AspNetCore.Mvc;

namespace forum.Controllers
{
    public class AuthController : Controller
    {
        private readonly ForumDbContext _context;

        public AuthController(ForumDbContext context)
        {
            _context = context;
        }

        public IActionResult SignIn()
        {
            return View();
        }
        
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult SignIn(IFormCollection form)
        {
            var email = form["email"].ToString();
            var password = form["password"].ToString();
            var user = _context.User.FirstOrDefault(u => u.email == email);

            if (user == null)
            {
                return Problem("Invalid credentials.");
            }

            var authResult = BCrypt.Net.BCrypt.Verify(password, user.passwordHash);

            if (!authResult)
            {
                return Problem("Invalid credentials.");
            }

            HttpContext.Session.SetString("email", email);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(IFormCollection form)
        {
            var email = form["email"].ToString();
            var password = form["password"].ToString();
            if (!new EmailAddressAttribute().IsValid(email))
            {
                return Problem("Invalid email address.");
            }
            if (password.Length < 6)
            {
                return Problem("Password must be at least 8 characters long.");
            }
            else if (password != form["confirmPassword"].ToString())
            {
                return Problem("Passwords do not match.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = _context.User.FirstOrDefault(u => u.email == email);

            if (user != null)
            {
                return Problem("User already exists.");
            }

            var newUser = new User
            {
                email = email,
                passwordHash = passwordHash,
                token = Guid.NewGuid().ToString(),
                role = "user",
                name = form["firstName"].ToString(),
                lastName = form["lastName"].ToString()
            };

            _context.Add(newUser);
            _context.SaveChanges();
            
            return RedirectToAction("AccountCreated");
        }
        
        public IActionResult AccountCreated()
        {
            return View();
        }
    }
}