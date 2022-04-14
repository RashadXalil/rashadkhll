using FinalBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using static FinalBack.Models.PaginatedList;

namespace FinalBack.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
    public class BlogCategoryController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogCategoryController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
        public IActionResult Index(int page = 1,bool? isDeleted=null)
        {
            ViewBag.IsDeleted = isDeleted;
            
            var BlogCategories = _context.BlogCategories.AsQueryable();

            if (isDeleted != null)
                BlogCategories = BlogCategories.Where(x => x.IsDeleted == isDeleted);

            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<BlogCategory>.Create(BlogCategories, page, pageSize));
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(BlogCategory category)
        {
            if (!ModelState.IsValid) return View();
            if(category.Name is null)
            {
                ModelState.AddModelError("Name", "Name file is required!");
                return View();
            }
            _context.BlogCategories.Add(category);
            _context.SaveChanges();
            TempData["Success"] = "Blog Category Created !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin,Admin,Editor")]
        public IActionResult Edit(int id)
        {
            BlogCategory category = _context.BlogCategories.FirstOrDefault(x => x.Id == id);

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(BlogCategory category)
        {
            if (!ModelState.IsValid)
                return View();

            BlogCategory existCategory = _context.BlogCategories.FirstOrDefault(x => x.Id == category.Id);
            if (existCategory == null) return NotFound();

            existCategory.Name = category.Name;

            _context.SaveChanges();
            TempData["Success"] = "Blog Category Edited !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            BlogCategory category = await _context.BlogCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null) return NotFound();
            category.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Blog Category Deleted !";
            return RedirectToAction("index");
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Restore(int id)
        {
            BlogCategory existcategory = _context.BlogCategories.FirstOrDefault(x => x.Id == id && x.IsDeleted);

            if (existcategory == null)
                return NotFound();

            existcategory.IsDeleted = false;
            _context.SaveChanges();
            TempData["Success"] = "Blog Category Restored !";
            return RedirectToAction("index");
        }
    }
}
