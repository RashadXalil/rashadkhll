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

    public class CategoryController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
        public IActionResult Index(int page = 1,bool? isDeleted=null)
        {
            ViewBag.isDeleted = isDeleted;
                
            var Categories = _context.Categories.AsQueryable();

            if (isDeleted != null)
                Categories = Categories.Where(x => x.IsDeleted == isDeleted).AsQueryable();

            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Category>.Create(Categories, page, pageSize));
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid) return View();
            if (category.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image file is required!");
                return View();
            }

            if (category.ImageFile.ContentType != "image/jpeg" && category.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                return View();
            }

            if (category.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                return View();
            }

            category.Image = Guid.NewGuid().ToString() + category.ImageFile.FileName;

            string path = Path.Combine(_env.WebRootPath, "uploads/categories", category.Image);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                category.ImageFile.CopyTo(stream);
            }

            _context.Categories.Add(category);
            _context.SaveChanges();
            TempData["Success"] = "Category Created !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin,Admin,Editor")]
        public IActionResult Edit(int id)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
                return Json(category);

            Category existcategory = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (existcategory == null) return NotFound();

            if (category.ImageFile != null)
            {
                if (category.ImageFile.ContentType != "image/jpeg" && category.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                    return View();
                }

                if (category.ImageFile.Length > 2097152)
                {

                    ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                    return View();
                }

                category.Image = Guid.NewGuid().ToString() + category.ImageFile.FileName;


                string path = Path.Combine(_env.WebRootPath, "uploads/categories", category.Image);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    category.ImageFile.CopyTo(stream);
                }

                if (existcategory.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/categories", existcategory.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);
                }

                existcategory.Image = category.Image;

            }
            else
            {
                if (category.Image == null && existcategory.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/categories", existcategory.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);

                    existcategory.Image = null;
                }
            }

            existcategory.Name = category.Name;

            _context.SaveChanges();
            TempData["Success"] = "Category Edited !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null) return NotFound();
            category.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category Deleted !";
            return RedirectToAction("index");
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Restore(int id)
        {
            Category existcategory = _context.Categories.FirstOrDefault(x => x.Id == id && x.IsDeleted);

            if (existcategory == null)
                return NotFound();

            existcategory.IsDeleted = false;
            _context.SaveChanges();
            TempData["Success"] = "Category Restored !";
            return RedirectToAction("index");
        }
    }
}
