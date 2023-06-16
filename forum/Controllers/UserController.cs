using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Forum.Data;
using Forum.Models;

namespace forum.Controllers
{
    public class UserController : Controller
    {
        private readonly ForumDbContext _context;

        public UserController(ForumDbContext context)
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

        // GET: User
        public async Task<IActionResult> Index()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }

            int? userId = user.id;
            string? role = user.role;

            if (userId == null || role == null || role != "admin")
            {
                return RedirectToAction("SignIn", "Auth");
            }

            return _context.User != null ?
                        View(await _context.User.ToListAsync()) :
                        Problem("Entity set 'ForumDbContext.User'  is null.");
        }

        public async Task<IActionResult> Profile()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }

            ViewBag.user = user;
            return View();
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var userSession = await GetUserAsync();
            if (userSession == null || userSession.id != id && userSession.role != "admin")
            {
                return RedirectToAction("SignIn", "Auth");
            }

            var user = await _context.User
                .FindAsync(id);

            return View(user);
        }

        // GET: User/Create
        public async Task<IActionResult> Create()
        {
            var userSession = await GetUserAsync();
            if (userSession == null || userSession.role != "admin")
            {
                return RedirectToAction("SignIn", "Auth");
            }

            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection userForm)
        {
            var userSession = await GetUserAsync();
            if (userSession == null || userSession.role != "admin")
            {
                return RedirectToAction("SignIn", "Auth");
            }

            if (userForm["password"].ToString().Length < 8)
            {
                return Problem("Password must be at least 8 characters long.");
            }

            var user = new User
            {
                name = userForm["name"].ToString(),
                lastName = userForm["lastName"].ToString(),
                email = userForm["email"].ToString(),
                passwordHash = BCrypt.Net.BCrypt.HashPassword(userForm["password"].ToString()),
                role = userForm["role"].ToString(),
                token = null
            };

            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var userSession = await GetUserAsync();
            if (userSession == null || userSession.id != id && userSession.role != "admin")
            {
                return RedirectToAction("SignIn", "Auth");
            }

            if (userSession.role == "admin")
            {
                ViewBag.role = "admin";
            }
            else
            {
                ViewBag.role = "user";
            }

            var user = await _context.User.FindAsync(id);

            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,lastName,email,passwordHash,token,role")] User user)
        {
            var userSession = await GetUserAsync();
            if (id != user.id || userSession == null || userSession.id != id && userSession.role != "admin")
            {
                return RedirectToAction("SignIn", "Auth");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (userSession.role != "admin")
                    return RedirectToAction(nameof(Profile));
                else
                    return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userSession = await GetUserAsync();
            if (id == null || _context.User == null || userSession == null || userSession.id != id && userSession.role != "admin")
            {
                return RedirectToAction("SignIn", "Auth");
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'ForumDbContext.User'  is null.");
            }
            var userSession = await GetUserAsync();
            if (userSession == null || userSession.id != id && userSession.role != "admin")
            {
                return RedirectToAction("SignIn", "Auth");
            }

            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
