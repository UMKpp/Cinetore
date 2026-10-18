using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.Models;
using System.Diagnostics;

namespace MovieStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieStoreContext _context;

        public HomeController(MovieStoreContext context)
        {
            _context = context;
        }

        // GET: /
        // Passes all movies to the home view so the hero + movie grid can render live DB data.
        public async Task<IActionResult> Index(string searchString)
        {
            var movies = from m in _context.Movies select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title.Contains(searchString));
            }

            ViewData["SearchString"] = searchString;
            return View(await movies.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
