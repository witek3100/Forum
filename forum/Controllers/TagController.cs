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
    public class TagController : Controller
    {
        private readonly ForumDbContext _context;

        public TagController(ForumDbContext context)
        {
            _context = context;
        }

        // GET: Tag
        public async Task<IActionResult> Index()
        {
            var userSession = await _context.User.FirstOrDefaultAsync((u) => u.token == HttpContext.Session.GetString("token"));
            if (userSession == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }

            return _context.Tag != null ?
                        View(await _context.Tag.ToListAsync()) :
                        Problem("Entity set 'ForumDbContext.Tag'  is null.");
        }

        // GET: Tag/Create
        public IActionResult Create()
        {
            var userSession = _context.User.FirstOrDefault((u) => u.token == HttpContext.Session.GetString("token"));
            if (userSession == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }

            return View();
        }

        // POST: Tag/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name")] Tag tag)
        {
            var userSession = await _context.User.FirstOrDefaultAsync((u) => u.token == HttpContext.Session.GetString("token"));
            if (userSession == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }

            if (ModelState.IsValid)
            {
                _context.Add(tag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        private bool TagExists(int id)
        {
            return (_context.Tag?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
