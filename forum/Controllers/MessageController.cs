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
    public class MessageController : Controller
    {
        private readonly ForumDbContext _context;

        public MessageController(ForumDbContext context)
        {
            _context = context;
        }

        // GET: Message
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("token") == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }

            var user = await _context.User.FirstOrDefaultAsync(u => u.token == HttpContext.Session.GetString("token"));

            if (user == null)
            {
                return Problem("User does not exist.");
            }

            var sentMessages = _context.Message.Where(m => m.senderEmail == user.email).ToList().OrderByDescending(m => m.createdAt);
            var recievedMessages = _context.Message.Where(m => m.receiverEmail == user.email).ToList().OrderByDescending(m => m.createdAt);
            ViewBag.sentMessages = sentMessages;
            ViewBag.recievedMessages = recievedMessages;
            return _context.Message != null ? View() : Problem("Entity set 'ForumDbContext.Message'  is null.");
        }

        // GET: Message/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Message/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,senderEmail,receiverEmail,content")] Message message)
        {
            string? token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }

            var user = _context.User.FirstOrDefault(u => u.token == token);

            if (user == null)
            {
                return NotFound();
            }

            message.senderEmail = user.email;

            if (message.senderEmail != "" && message.receiverEmail != "")
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        private bool MessageExists(int id)
        {
            return (_context.Message?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
