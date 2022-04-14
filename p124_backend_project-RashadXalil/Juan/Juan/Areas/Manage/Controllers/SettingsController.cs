using Juan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Juan.Models.PaginatedList;

namespace Juan.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin")]

    public class SettingsController : Controller
    {
        private readonly JuanContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingsController(JuanContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Settings>.Create(_context.Settings.AsQueryable(), page, pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Settings setting)
        {
            if (setting.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Image file is required!");

            if (setting.ImageFile.ContentType != "image/jpeg" && setting.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                return View();
            }

            if (setting.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                return View();
            }

            setting.Image = Guid.NewGuid().ToString() + setting.ImageFile.FileName;

            string path = Path.Combine(_env.WebRootPath, "uploads/brands", setting.Image);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                setting.ImageFile.CopyTo(stream);
            }

            _context.Settings.Add(setting);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
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
        public IActionResult Delete(int id)
        {
            Settings setting = _context.Settings.FirstOrDefault(x => x.Id == id);

            return View(setting);
        }

        [HttpPost]
        public IActionResult Delete(Settings setting)
        {
            Settings existSetting = _context.Settings.FirstOrDefault(x => x.Id == setting.Id);

            if (existSetting == null)
                return NotFound();

            _context.Settings.Remove(existSetting);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

    }
}
