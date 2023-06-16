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
    public class CommentController : Controller
    {
        private readonly ForumDbContext _context;

        public CommentController(ForumDbContext context)
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


        // GET: Comment
        public async Task<IActionResult> Index()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }

            string? role = user.role;

            if (role != "admin")
            {
                return RedirectToAction("SignIn", "Auth");
            }

            return _context.Comment != null ?
                        View(await _context.Comment.ToListAsync()) :
                        Problem("Entity set 'ForumDbContext.Comment' is null.");
        }

        // GET: Comment/Create
        public async Task<IActionResult> Create(int id)
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }
            ViewBag.postId = id;
            return View();
        }

        // POST: Comment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("postId,content")] Comment comment)
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }
            var postId = comment.postId;
            comment.userId = user.id;
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("PostComments", "Comment", new { id = postId });
            }
            return View(comment);
        }

        // GET: Comment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var user = await GetUserAsync();

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.id == id);
            if (comment == null)
            {
                return NotFound();
            }

            if (user == null || user.role != "admin" && user.id != comment.userId)
            {
                return RedirectToAction("SignIn", "Auth");
            }

            return View(comment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comment == null)
            {
                return Problem("Entity set 'ForumDbContext.Comment'  is null.");
            }
            var comment = await _context.Comment.FindAsync(id);

            var user = await GetUserAsync();

            if (comment != null)
            {
                if (user == null || user.role != "admin" && user.id != comment.userId)
                {
                    return RedirectToAction("SignIn", "Auth");
                }
                _context.Comment.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return (_context.Comment?.Any(e => e.id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> PostComments(int id)
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }

            var comments = _context.Comment.Where(c => c.postId == id).ToList().OrderByDescending(p => p.createdAt);
            var post = _context.Post.Where(p => p.id == id).ToList()[0];
            ViewBag.comments = comments;
            ViewBag.post = post;
            return View();
        }
    }
}
