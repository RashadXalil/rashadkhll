
using FinalBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static FinalBack.Models.PaginatedList;

namespace FinalBack.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]


    public class BranchController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public BranchController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
        public IActionResult Index(int page = 1,bool? isDeleted=null)
        {
            ViewBag.isDeleted = isDeleted;
            var Branches = _context.Branches.AsQueryable();
            if (isDeleted != null)
                Branches = Branches.Where(x => x.IsDeleted == isDeleted).AsQueryable();
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Branch>.Create(Branches, page, pageSize));
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Branch branch)
        {
            if (!ModelState.IsValid) return View();

            if (branch.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image file is required!");
                return View();
            }

            if (branch.ImageFile.ContentType != "image/jpeg" && branch.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                return View();
            }

            if (branch.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                return View();
            }

            branch.Image = Guid.NewGuid().ToString() + branch.ImageFile.FileName;

            string path = Path.Combine(_env.WebRootPath, "uploads/branches", branch.Image);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                branch.ImageFile.CopyTo(stream);
            }

            _context.Branches.Add(branch);
            _context.SaveChanges();
            TempData["Success"] = "Branch Created !";
            return RedirectToAction("index");


        }
        [Authorize(Roles = "SuperAdmin,Admin,Editor")]
        public IActionResult Edit(int id)
        {
            Branch branch = _context.Branches.FirstOrDefault(x => x.Id == id);
            if (branch == null)
                return NotFound();

            return View(branch);
        }

        [HttpPost]
        public IActionResult Edit(Branch branch)
        {
            if (!ModelState.IsValid)
                return Json(branch);

            Branch existbranch = _context.Branches.FirstOrDefault(x => x.Id == branch.Id);
            if (existbranch == null) return NotFound();

            if (branch.ImageFile != null)
            {
                if (branch.ImageFile.ContentType != "image/jpeg" && branch.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                    return View();
                }

                if (branch.ImageFile.Length > 2097152)
                {

                    ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                    return View();
                }

                branch.Image = Guid.NewGuid().ToString() + branch.ImageFile.FileName;


                string path = Path.Combine(_env.WebRootPath, "uploads/branches", branch.Image);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    branch.ImageFile.CopyTo(stream);
                }

                if (existbranch.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/branches", existbranch.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);
                }

                existbranch.Image = branch.Image;

            }
            else
            {
                if (branch.Image == null && existbranch.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/brances", existbranch.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);

                    existbranch.Image = null;
                }
            }
            existbranch.Name = branch.Name;
            existbranch.Location = branch.Location;
            existbranch.Mail = branch.Mail;
            existbranch.Phone = branch.Phone;
            existbranch.ModifiedAt = DateTime.Now;
            _context.SaveChanges();
            TempData["Success"] = "Branch Edited !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            Branch branch = await _context.Branches.FirstOrDefaultAsync(x => x.Id == id);
            if (branch is null) return NotFound();
            branch.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Branch Deleted !";
            return RedirectToAction("index");
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Restore(int id)
        {
            Branch existbranch = _context.Branches.FirstOrDefault(x => x.Id == id && x.IsDeleted);

            if (existbranch == null)
                return NotFound();

            existbranch.IsDeleted = false;
            _context.SaveChanges();
            TempData["Success"] = "Branch Restored !";
            return RedirectToAction("index");
        }
    }
}
