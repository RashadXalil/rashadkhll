using FinalBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FinalBack.Models.PaginatedList;

namespace FinalBack.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]


    public class SubCategoryController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public SubCategoryController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
        public IActionResult Index(int page = 1,bool? isDeleted=null)
        {
            ViewBag.isDeleted = isDeleted;
            var subcategories = _context.SubCategories.Include(x => x.Category).AsQueryable();
            if (isDeleted != null)
                subcategories = subcategories.Where(x => x.IsDeleted == isDeleted);
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<SubCategory>.Create(subcategories, page, pageSize));
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator")]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.Where(x => !x.IsDeleted).ToList();

            return View();
        }
        [HttpPost]
        public IActionResult Create(SubCategory subcategory)
        {
            if (!ModelState.IsValid) return View();
            ViewBag.Categories = _context.Categories.Where(x => !x.IsDeleted).ToList();

            _context.SubCategories.Add(subcategory);
            _context.SaveChanges();
            TempData["Success"] = "Subcategory Created !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin,Admin,Editor")]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _context.Categories.Where(x => !x.IsDeleted).ToList();

            SubCategory category = _context.SubCategories.FirstOrDefault(x => x.Id == id);

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(SubCategory subcategory)
        {
            ViewBag.Categories = _context.Categories.Where(x => !x.IsDeleted).ToList();

            if (!ModelState.IsValid)
                return View();

            SubCategory existCategory = _context.SubCategories.FirstOrDefault(x => x.Id == subcategory.Id);
            if (existCategory == null) return NotFound();

            existCategory.Name = subcategory.Name;
            existCategory.CategoryId = subcategory.CategoryId;
            _context.SaveChanges();
            TempData["Success"] = "Subcategory Edited !";

            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            SubCategory category = await _context.SubCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null) return NotFound();
            category.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Subcategory Deleted !";

            return RedirectToAction("index");
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Restore(int id)
        {
            SubCategory existcategory = _context.SubCategories.FirstOrDefault(x => x.Id == id && x.IsDeleted);

            if (existcategory == null)
                return NotFound();

            existcategory.IsDeleted = false;
            _context.SaveChanges();
            TempData["Success"] = "Subcategory Restored !";

            return RedirectToAction("index");
        }
    }
}
