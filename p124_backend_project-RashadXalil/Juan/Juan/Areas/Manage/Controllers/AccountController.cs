using Juan.Areas.Manage.ViewModels;
using Juan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly JuanContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(JuanContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        //public async Task<IActionResult> test()
        //{
        //    AppUser appUser = new AppUser
        //    {
        //        FullName = "Super Admin",
        //        UserName = "SuperAdmin",
        //        Email = "superadmin@gmail.com"
        //    };

        //   var result = await _userManager.CreateAsync(appUser, "Superadmin123.");s

        //    return Ok(result.Errors);
        //}

        //public async Task<IActionResult> Test()
        //{
        //    var result1 = await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    var result2 = await _roleManager.CreateAsync(new IdentityRole("Member"));
        //    var result3 = await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

        //    AppUser admin = await _userManager.FindByNameAsync("SuperAdmin");

        //    var result = await _userManager.AddToRoleAsync(admin, "SuperAdmin");

        //    return Ok();
        //}

        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(AdminLoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View();

            AppUser admin = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginVM.UserName && x.IsAdmin == true);
            if (admin == null)
            {
                ModelState.AddModelError("", "UserName or Password is incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(admin, loginVM.Password,true,false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password is incorrect");
                return View();
            }

            return RedirectToAction("index", "dashboard");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("adminlogin", "account");
        }
    }
}
