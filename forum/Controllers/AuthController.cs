using System.ComponentModel.DataAnnotations;
using Forum.Data;
using Forum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace forum.Controllers
{
    public class AuthController : Controller
    {
        private readonly ForumDbContext _context;

        public AuthController(ForumDbContext context)
        {
            _context = context;
        }

        private async Task<User?> GetUserAsync()
        {
            string? token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return null;
            }
            var user = await _context.User.FirstOrDefaultAsync(u => u.token == token);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {   

            var user = await GetUserAsync();
            if (user != null)
            {
                user.token = null;
                _context.User.Update(user);
                _context.SaveChanges();
            }
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

            string token = Guid.NewGuid().ToString();

            HttpContext.Session.SetString("token", token);

            user.token = token;
            _context.User.Update(user);
            _context.SaveChanges();

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
                token = null,
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

        // public IActionResult AccessDenied()
        // {
        //     return View();
        // }

        // public IActionResult ForgotPassword()
        // {
        //     return View();
        // }

        // [HttpPost]
        // public IActionResult ForgotPassword(IFormCollection form)
        // {
        //     var email = form["email"].ToString();
        //     var user = _context.User.FirstOrDefault(u => u.email == email);

        //     if (user == null)
        //     {
        //         return Problem("User does not exist.");
        //     }

        //     var newPassword = Guid.NewGuid().ToString().Substring(0, 8);
        //     var passwordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        //     user.passwordHash = passwordHash;
        //     _context.SaveChanges();

        //     return RedirectToAction("PasswordReset", new { newPassword = newPassword });
        // }

        public IActionResult ChangePassword()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToAction("SignIn");
            }

            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(IFormCollection form)
        {
            var token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToAction("SignIn");
            }

            var user = _context.User.FirstOrDefault(u => u.token == token);
            var oldPassword = form["oldPassword"].ToString();
            var newPassword = form["newPassword"].ToString();
            var confirmPassword = form["confirmPassword"].ToString();

            if (user == null)
            {
                return Problem("User does not exist.");
            }

            var authResult = BCrypt.Net.BCrypt.Verify(oldPassword, user.passwordHash);

            if (!authResult)
            {
                return Problem("Invalid credentials.");
            }

            if (newPassword.Length < 8)
            {
                return Problem("Password must be at least 8 characters long.");
            }
            else if (newPassword != confirmPassword)
            {
                return Problem("Passwords do not match.");
            }
            else if (newPassword == oldPassword)
            {
                return Problem("New password must be different from the old one.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.passwordHash = passwordHash;
            user.token = null;
            _context.User.Update(user);
            _context.SaveChanges();
            HttpContext.Session.Clear();

            return RedirectToAction("PasswordChanged");
        }

        public IActionResult PasswordChanged()
        {
            return View();
        }
    }
}