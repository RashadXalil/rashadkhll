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

    public class ColorController : Controller
    {
        private readonly JuanContext _context;
        private readonly IWebHostEnvironment _env;

        public ColorController(JuanContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Color>.Create(_context.Colors.AsQueryable(), page, pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Color color)
        {

            _context.Colors.Add(color);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
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
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Color color = _context.Colors.FirstOrDefault(x => x.Id == id);

            return View(color);
        }

        [HttpPost]
        public IActionResult Delete(Color color)
        {
            Color existColor = _context.Colors.FirstOrDefault(x => x.Id == color.Id);

            if (existColor == null)
                return NotFound();

            _context.Colors.Remove(existColor);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

    }
}
