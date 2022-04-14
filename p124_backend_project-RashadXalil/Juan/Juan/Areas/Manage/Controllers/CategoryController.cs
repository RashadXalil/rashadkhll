using Juan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Juan.Models.PaginatedList;

namespace Juan.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin")]

    public class CategoryController : Controller
    {
        private readonly JuanContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(JuanContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Category>.Create(_context.Categories.AsQueryable(), page, pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
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
                return View();

            Category existCategory = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (existCategory == null) return NotFound();

            existCategory.Name = category.Name;

            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);

            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(Category category)
        {
            Category existCategory = _context.Categories.FirstOrDefault(x => x.Id == category.Id);

            if (existCategory == null)
                return NotFound();

            _context.Categories.Remove(existCategory);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
