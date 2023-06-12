using System.Diagnostics;
using Forum.Data;
using Microsoft.AspNetCore.Mvc;
using forum.Models;
using Forum.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SQLitePCL;

namespace forum.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ForumDbContext _context;

    public HomeController(ILogger<HomeController> logger, ForumDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        
        if (HttpContext.Session.GetString("email") == null)
        {
            return View();
        }

        List<Post> posts = new List<Post>();
        posts = _context.Post.OrderByDescending(p => p.createdAt).ToList();
        ViewBag.posts = posts;
        ViewBag.userEmail = HttpContext.Session.GetString("email");
        return View("IndexLoggedIn");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
