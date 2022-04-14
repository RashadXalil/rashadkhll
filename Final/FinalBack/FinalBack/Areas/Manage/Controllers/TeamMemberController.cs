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
    public class TeamMemberController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamMemberController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
        public IActionResult Index(int page = 1, bool? isDeleted = null)
        {
            ViewBag.isDeleted = isDeleted;
            var TeamMembers = _context.TeamMembers.AsQueryable();
            if (isDeleted != null)
                TeamMembers = TeamMembers.Where(x => x.IsDeleted == isDeleted).AsQueryable();
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<TeamMember>.Create(TeamMembers, page, pageSize));
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(TeamMember member)
        {
            if (!ModelState.IsValid) return View();

            if (member.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image file is required!");
                return View();  
            }

            if (member.ImageFile.ContentType != "image/jpeg" && member.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                return View();
            }

            if (member.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                return View();
            }

            member.Image = Guid.NewGuid().ToString() + member.ImageFile.FileName;

            string path = Path.Combine(_env.WebRootPath, "uploads/teammembers", member.Image);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                member.ImageFile.CopyTo(stream);
            }

            _context.TeamMembers.Add(member);
            _context.SaveChanges();
            TempData["Success"] = "Team Member Created !";

            return RedirectToAction("index");


        }
        [Authorize(Roles = "SuperAdmin,Admin,Editor")]
        public IActionResult Edit(int id)
        {
            TeamMember member = _context.TeamMembers.FirstOrDefault(x => x.Id == id);
            if (member == null)
                return NotFound();

            return View(member);
        }

        [HttpPost]
        public IActionResult Edit(TeamMember member)
        {
            if (!ModelState.IsValid)
                return Json(member);

            TeamMember existmember = _context.TeamMembers.FirstOrDefault(x => x.Id == member.Id);
            if (existmember == null) return NotFound();

            if (member.ImageFile != null)
            {
                if (member.ImageFile.ContentType != "image/jpeg" && member.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                    return View();
                }

                if (member.ImageFile.Length > 2097152)
                {

                    ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                    return View();
                }

                member.Image = Guid.NewGuid().ToString() + member.ImageFile.FileName;


                string path = Path.Combine(_env.WebRootPath, "uploads/teammembers", member.Image);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    member.ImageFile.CopyTo(stream);
                }

                if (existmember.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/teammembers", existmember.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);
                }

                existmember.Image = member.Image;

            }
            else
            {
                if (member.Image == null && existmember.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/brances", existmember.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);

                    existmember.Image = null;
                }
            }
            existmember.Name = member.Name;
            existmember.Surname = member.Surname;
            existmember.DribbleURL = member.DribbleURL;
            existmember.FacebookURL = member.FacebookURL;
            existmember.TwitterURl = member.TwitterURl;
            existmember.YoutubeURL = member.YoutubeURL;
            _context.SaveChanges();
            TempData["Success"] = "Team Member Edited !";

            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            TeamMember member = await _context.TeamMembers.FirstOrDefaultAsync(x => x.Id == id);
            if (member is null) return NotFound();
            member.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Team Member Deleted !";

            return RedirectToAction("index");
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Restore(int id)
        {
            TeamMember existmember = _context.TeamMembers.FirstOrDefault(x => x.Id == id && x.IsDeleted);

            if (existmember == null)
                return NotFound();

            existmember.IsDeleted = false;
            _context.SaveChanges();
            TempData["Success"] = "Team Member Restored !";
            return RedirectToAction("index");
        }
    }
}
