using Juan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Juan.Models.PaginatedList;

namespace Juan.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin")]

    public class ReviewController : Controller
    {
        private readonly JuanContext _context;
        private readonly IWebHostEnvironment _env;

        public ReviewController(JuanContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var reviews = _context.Reviews.Include(x=>x.AppUser).Include(x=>x.Product).AsQueryable();
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Review>.Create(reviews, page, pageSize));
        }
        public IActionResult Delete(int id)
        {
            Review review = _context.Reviews.FirstOrDefault(x => x.Id == id);

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

            return RedirectToAction("index");
        }

    }
}
