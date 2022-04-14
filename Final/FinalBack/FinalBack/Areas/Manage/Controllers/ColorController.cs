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



    public class ColorController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public ColorController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1,bool? isDeleted=null)
        {
            ViewBag.isDeleted = isDeleted;

            var Colors = _context.Colors.AsQueryable();

            if (isDeleted != null)
                Colors = Colors.Where(x => x.IsDeleted == isDeleted).AsQueryable();
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Color>.Create(Colors, page, pageSize));
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Color color)
        {
            if (!ModelState.IsValid) return View();
            _context.Colors.Add(color);
            _context.SaveChanges();
            TempData["Success"] = "Color Created !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin,Admin,Editor")]
        public IActionResult Edit(int id)
        {
            Color color = _context.Colors.FirstOrDefault(x => x.Id == id);

            if (color == null) return NotFound();

            return View(color);
        }

        [HttpPost]
        public IActionResult Edit(Color color)
        {
            if (!ModelState.IsValid)
                return View();

            Color existColor = _context.Colors.FirstOrDefault(x => x.Id == color.Id);
            if (existColor == null) return NotFound();

            existColor.Name = color.Name;
            existColor.HexCode = color.HexCode;
            _context.SaveChanges();
            TempData["Success"] = "Color Edited !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            Color color = await _context.Colors.FirstOrDefaultAsync(x => x.Id == id);
            if (color is null) return NotFound();
            color.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Color Deleted !";
            return RedirectToAction("index");
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Restore(int id)
        {
            Color existcolor = _context.Colors.FirstOrDefault(x => x.Id == id && x.IsDeleted);

            if (existcolor == null)
                return NotFound();

            existcolor.IsDeleted = false;
            _context.SaveChanges();
            TempData["Success"] = "Color Restored !";
            return RedirectToAction("index");
        }

    }
}
