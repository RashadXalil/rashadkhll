using FinalBack.Areas.Manage.ViewModels;
using FinalBack.Models;
using FinalBack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FinalBack.Models.PaginatedList;

namespace FinalBack.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly FinalContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(FinalContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Index(int page = 1 )
        {
            var Admins = _context.AppUsers.Where(x=>x.IsAdmin==true && x.UserName!="SuperAdmin").AsQueryable();
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);

            return View(PagenatedList<AppUser>.Create(Admins, page, pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AdminCreateViewModel adminVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser member = await _userManager.FindByNameAsync(adminVM.UserName);
            if (member != null)
            {
                ModelState.AddModelError("Username", "Username already exist !");
                return View();
            }
            member = new AppUser
            {
                Name = adminVM.Name,
                Surname = adminVM.Surname,
                UserName = adminVM.UserName,
                Email = adminVM.Email,
                FullName = adminVM.Name + " " + adminVM.Surname,
                IsAdmin = true,
            };

            var result = await _userManager.CreateAsync(member, adminVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            var roleResult = await _userManager.AddToRoleAsync(member, adminVM.Role);

            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            TempData["Success"] = "Succesfully Created !";

            return RedirectToAction("index");
        }

        public async  Task<IActionResult> Edit(string userId)
        {
            if (!ModelState.IsValid) return View();

            AppUser admin =await _userManager.FindByIdAsync(userId);
            if (admin is null) return RedirectToAction("error", "home");
            var roles = _userManager.GetRolesAsync(admin);
            var role = "";
            foreach (var item in roles.Result)
            {
                role = item;
            }

            

            AdminEditViewModel adminVM = new AdminEditViewModel()
            {
                AdminId = admin.Id,
                CurrentRole = role,
            };

            ViewBag.AdminId = admin.Id;
            ViewBag.Role = role;
            return View(adminVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminEditViewModel adminVM)
        {

            var admin = await _userManager.FindByIdAsync(adminVM.AdminId);

            if (admin is null) return NotFound();

            var removeresult =await _userManager.RemoveFromRoleAsync(admin,adminVM.CurrentRole);
            var roleresult =await _userManager.AddToRoleAsync(admin, adminVM.NewRole);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Admin Edited !";
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(string username)
        {
            AppUser user =await _userManager.FindByNameAsync(username);

            if (user is null) return NotFound();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AppUser user)
        {
            AppUser existUser =await _userManager.FindByIdAsync(user.Id);

            if (existUser == null)
                return NotFound();

            _context.AppUsers.Remove(existUser);
            _context.SaveChanges();
            TempData["Success"] = "Admin Deleted !";
            return RedirectToAction("index");
        }
    }
}
