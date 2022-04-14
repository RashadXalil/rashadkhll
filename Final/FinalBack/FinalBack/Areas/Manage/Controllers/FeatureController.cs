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


    public class FeatureController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public FeatureController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
        public IActionResult Index(int page = 1, bool? isDeleted = null)
        {
            ViewBag.IsDeleted = isDeleted;

            var Features = _context.Features.AsQueryable();

            if (isDeleted != null)
                Features = Features.Where(x => x.IsDeleted == isDeleted);

            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Feature>.Create(Features, page, pageSize));
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Feature feature)
        {
            if (!ModelState.IsValid) return View();

            _context.Features.Add(feature);
            _context.SaveChanges();
            TempData["Success"] = "Feature Created !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin,Admin,Editor")]
        public IActionResult Edit(int id)
        {
            Feature feature = _context.Features.FirstOrDefault(x => x.Id == id);

            if (feature == null) return NotFound();
            TempData["Success"] = "Feature Edited !";
            return View(feature);
        }

        [HttpPost]
        public IActionResult Edit(Feature feature)
        {
            if (!ModelState.IsValid)
                return View();

            Feature existFeature = _context.Features.FirstOrDefault(x => x.Id == feature.Id);
            if (existFeature == null) return NotFound();
            existFeature.Title = feature.Title;
            existFeature.Description = feature.Description;
            existFeature.Image = feature.Image;
            _context.SaveChanges();
            TempData["Success"] = "Feature Edited !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            Feature feature = await _context.Features.FirstOrDefaultAsync(x => x.Id == id);
            if (feature is null) return NotFound();
            feature.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Feature Deleted !";
            return RedirectToAction("index");
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Restore(int id)
        {
            Feature existfeature = _context.Features.FirstOrDefault(x => x.Id == id && x.IsDeleted);

            if (existfeature == null)
                return NotFound();

            existfeature.IsDeleted = false;
            _context.SaveChanges();
            TempData["Success"] = "Feature Restored !";
            return RedirectToAction("index");
        }

    }
}
