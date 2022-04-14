using FinalBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static FinalBack.Models.PaginatedList;

namespace FinalBack.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
    public class BlogController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
        public IActionResult Index(int page = 1,bool? isDeleted=null)
        {
            ViewBag.isDeleted = isDeleted;

            var Blogs = _context.Blogs.Include(x => x.BlogCategory).AsQueryable();

            if (isDeleted != null)
                Blogs = _context.Blogs.Include(x => x.BlogCategory).Where(x => x.IsDeleted == isDeleted).AsQueryable();

            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Blog>.Create(Blogs, page, pageSize));
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator")]
        public IActionResult Create()
        {
            ViewBag.BlogCategories = _context.BlogCategories.Where(x => !x.IsDeleted).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Blog blog)
        {
            if (!ModelState.IsValid) return View();
            ViewBag.BlogCategories = _context.BlogCategories.Where(x => !x.IsDeleted).ToList();

            if (blog.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image file is required!");
                return View();
            }

            if (blog.ImageFile.ContentType != "image/jpeg" && blog.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                return View();
            }

            if (blog.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                return View();
            }

            blog.Image = Guid.NewGuid().ToString() + blog.ImageFile.FileName;

            string path = Path.Combine(_env.WebRootPath, "uploads/blogs", blog.Image);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                blog.ImageFile.CopyTo(stream);
            }
            _context.Blogs.Add(blog);
            _context.SaveChanges();
            TempData["Success"] = "Blog Created !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin,Admin,Editor")]
        public IActionResult Edit(int id)
        {
            Blog blog = _context.Blogs.Include(x => x.BlogCategory).FirstOrDefault(x => x.Id == id );
            if (blog == null)
                return NotFound();
            ViewBag.BlogCategories = _context.BlogCategories.ToList();

            return View(blog);
        }

        [HttpPost]
        public IActionResult Edit(Blog blog)
        {
            ViewBag.BlogCategories = _context.BlogCategories.ToList();
            if (!ModelState.IsValid) return NotFound();


            Blog existblog = _context.Blogs.Include(x => x.BlogCategory).FirstOrDefault(x => x.Id == blog.Id);
            if (existblog == null) return NotFound();

            if (blog.ImageFile != null)
            {
                if (blog.ImageFile.ContentType != "image/jpeg" && blog.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                    return View();
                }

                if (blog.ImageFile.Length > 2097152)
                {

                    ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                    return View();
                }

                blog.Image = Guid.NewGuid().ToString() + blog.ImageFile.FileName;


                string path = Path.Combine(_env.WebRootPath, "uploads/blogs", blog.Image);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    blog.ImageFile.CopyTo(stream);
                }

                if (existblog.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/blogs", existblog.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);
                }

                existblog.Image = blog.Image;

            }
            else
            {
                if (blog.Image == null && existblog.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/blogs", existblog.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);

                    existblog.Image = null;
                }
            }
            existblog.Title1 = blog.Title1;
            existblog.Title2 = blog.Title2;
            existblog.Desc1 = blog.Desc1;
            existblog.Desc2 = blog.Desc2;
            existblog.IsPopular = blog.IsPopular;
            existblog.Desc3 = blog.Desc3;
            existblog.ModifiedAt = DateTime.UtcNow.AddHours(4);
            _context.SaveChanges();
            TempData["Success"] = "Blog Edited !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return View();
            Blog blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (blog is null) return NotFound();
            blog.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Blog Deleted !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Restore(int id)
        {
            Blog existblog = _context.Blogs.FirstOrDefault(x => x.Id == id && x.IsDeleted);

            if (existblog == null)
                return NotFound();

            existblog.IsDeleted = false;
            _context.SaveChanges();
            TempData["Success"] = "Blog Restored !";
            return RedirectToAction("index");
        }

    }
}
