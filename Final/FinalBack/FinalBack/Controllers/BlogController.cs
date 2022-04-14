using FinalBack.Models;
using FinalBack.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FinalBack.Controllers
{
    public class BlogController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int? catId,int page=1)
        {
            ViewBag.PageIndex = page;
            ViewBag.CategoryId = catId;
            var Blogs = _context.Blogs.Include(x=>x.BlogCategory).Where(x => x.IsDeleted == false).AsQueryable();
            if (catId != null)
                Blogs = Blogs.Where(x => x.BlogCategoryId == catId).AsQueryable();
            ViewBag.TotalPage = (int)Math.Ceiling(Blogs.Count() / 2d);
            HomeViewModel homeVM = new HomeViewModel()
            {
                Blogs = Blogs.Skip((page - 1) * 2).Take(2).ToList(),
                BlogCategories = _context.BlogCategories.Include(x => x.Blogs).Where(x => x.IsDeleted == false).ToList(),
                PopularBlogs = _context.Blogs.Where(x => x.IsPopular == true).ToList()
                
            };
            return View(homeVM);
        }
        public IActionResult Detail(int BlogId)
        {
            Blog blog = _context.Blogs.Include(x => x.BlogCategory).Where(x=>x.IsDeleted==false).FirstOrDefault(x => x.Id == BlogId);
            if (blog is null) return RedirectToAction("error", "home");
            HomeViewModel homeVM = new HomeViewModel()
            {
                Blog = _context.Blogs.Include(x => x.BlogCategory).FirstOrDefault(x => x.Id == BlogId),
                RelatedBlogs = _context.Blogs.Include(x => x.BlogCategory).Where(x => x.BlogCategoryId == blog.BlogCategoryId && x.IsDeleted==false).ToList(),
                BlogCategories = _context.BlogCategories.Include(x => x.Blogs).Where(x => x.IsDeleted == false).ToList(),
                PopularBlogs = _context.Blogs.Include(x=>x.BlogCategory).Where(x => x.IsPopular == true && x.IsDeleted==false).ToList()
            };
            blog.View += 1;
            _context.SaveChanges();
            return View(homeVM);
        }
    }
}
