using FinalBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static FinalBack.Models.PaginatedList;

namespace FinalBack.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin,SuperAdmin")]

    public class SettingsController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingsController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Settings>.Create(_context.Settings.AsQueryable(), page, pageSize));
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Settings setting)
        {
            if (!ModelState.IsValid) return View();
            _context.Settings.Add(setting);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Edit(int id)
        {
            Settings setting = _context.Settings.FirstOrDefault(x => x.Id == id);

            if (setting == null) return NotFound();

            return View(setting);
        }

        [HttpPost]
        public IActionResult Edit(Settings setting)
        {
            if (!ModelState.IsValid)
                return View();

            Settings existSetting = _context.Settings.FirstOrDefault(x => x.Id == setting.Id);
            if (existSetting == null) return NotFound();

            existSetting.Key = setting.Key;
            existSetting.Value = setting.Value;

            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
