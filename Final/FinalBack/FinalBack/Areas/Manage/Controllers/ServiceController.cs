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



    public class ServiceController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public ServiceController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
        public IActionResult Index(int page = 1,bool? isDeleted=null)
        {
            ViewBag.isDeleted = isDeleted;

            var Services = _context.Services.AsQueryable();

            if (isDeleted != null)
                Services = Services.Where(x => x.IsDeleted == isDeleted).AsQueryable();

            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Service>.Create(Services, page, pageSize));
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Service service)
        {
            if (!ModelState.IsValid) return NotFound();
            _context.Services.Add(service);
            _context.SaveChanges();
            TempData["Success"] = "Service Created !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin,Admin,Editor")]
        public IActionResult Edit(int id)
        {
            Service service = _context.Services.FirstOrDefault(x => x.Id == id);
            if (service == null) return NotFound();
            return View(service);
        }

        [HttpPost]
        public IActionResult Edit(Service service)
        {
            if (!ModelState.IsValid)
                return View();

            Service existService = _context.Services.FirstOrDefault(x => x.Id == service.Id);
            if (existService == null) return NotFound();
            existService.Title = service.Title;
            existService.Desc = service.Desc;
            existService.Image = service.Image;
            _context.SaveChanges();
            TempData["Success"] = "Service Edited !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            Service service = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (service is null) return NotFound();
            service.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Service Deleted !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]

        public IActionResult Restore(int id)
        {
            Service existservice = _context.Services.FirstOrDefault(x => x.Id == id && x.IsDeleted);

            if (existservice == null)
                return NotFound();

            existservice.IsDeleted = false;
            _context.SaveChanges();
            TempData["Success"] = "Service Restored !";
            return RedirectToAction("index");
        }

    }
}
