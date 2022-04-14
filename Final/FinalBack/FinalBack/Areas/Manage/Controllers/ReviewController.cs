using FinalBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static FinalBack.Models.PaginatedList;

namespace FinalBack.Areas.Manage.Controllers
{

    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]

    public class ReviewController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public ReviewController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
        public IActionResult Index(int page = 1)
        {
            var reviews = _context.Reviews.Include(x => x.AppUser).Include(x => x.Product).AsQueryable();
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Review>.Create(reviews, page, pageSize));
        }
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Delete(int id)
        {
            Review review = _context.Reviews.FirstOrDefault(x => x.Id == id);
            if (review is null) return NotFound();
            return View(review);
        }

        [HttpPost]
        public IActionResult Delete(Review review)
        {
            Review existReview = _context.Reviews.FirstOrDefault(x => x.Id == review.Id);

            if (existReview == null)
                return NotFound();

            _context.Reviews.Remove(existReview);
            _context.SaveChanges();
            TempData["Success"] = "Review Deleted !";
            return RedirectToAction("index");
        }

    }
}
