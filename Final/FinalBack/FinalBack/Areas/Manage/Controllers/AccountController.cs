    using FinalBack.Areas.Manage.ViewModels;
using FinalBack.Models;
using FinalBack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FinalBack.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly FinalContext  _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(FinalContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
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

        //    var result = await _userManager.CreateAsync(appUser, "Superadmin123.");

        //    return Ok(result.Errors);
        //}

        //public async Task<IActionResult> Test()
        //{
        //    var result1 = await _roleManager.CreateAsync(new IdentityRole("Editor"));
        //    var result2 = await _roleManager.CreateAsync(new IdentityRole("Creator"));

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

            var result = await _signInManager.PasswordSignInAsync(admin, loginVM.Password, true, false);

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
        public async Task<IActionResult> Profile()
        {
            AppUser member = await _userManager.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            if (member is null) return RedirectToAction("Error", "Home");

            AdminProfileViewModel profileVM = new AdminProfileViewModel
            {
              Email = member.Email,
              Fullname = member.FullName.ToUpper(),
              Country = member.Country,
              UserName = member.UserName,
              Adress = member.Address,
              City = member.City,
              Phonenumber = member.PhoneNumber,
            };
            return View(profileVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AdminUpdateViewModel adminVM)
        {
            AppUser member = await _userManager.FindByNameAsync(User.Identity.Name);

            if (adminVM == null) return NotFound();

            AdminProfileViewModel profileVM = new AdminProfileViewModel
            {
                Email = adminVM.Email,
                Fullname = adminVM.FullName,
                Country = member.Country,
                UserName = member.UserName,
                Adress = member.Address,
            };
            if (!ModelState.IsValid)
                return View("profile", profileVM);

            if (member.UserName != adminVM.UserName)
            {
                if (_userManager.Users.Any(x => x.NormalizedUserName == adminVM.UserName.ToUpper()))
                {
                    ModelState.AddModelError("Username", "Username already exist");
                    return View("profile", profileVM);
                }
            }
            if (member.Email != adminVM.Email)
            {
                if (_userManager.Users.Any(x => x.NormalizedEmail == adminVM.Email.ToUpper()))
                {
                    ModelState.AddModelError("Email", "Email already exist");
                    return View("profile", profileVM);
                }
            }


            member.Email = adminVM.Email;
            member.FullName = adminVM.FullName;
            member.Country = adminVM.Country;
            member.City = adminVM.City;
            member.Address = adminVM.Adress;
            member.PhoneNumber = adminVM.Phonenumber;
            member.UserName = adminVM.UserName;

            var result = await _userManager.UpdateAsync(member);
            if (adminVM.Password != null)
            {
                if (string.IsNullOrWhiteSpace(adminVM.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Current Password required !");
                    return View("profile", profileVM);
                }

                if (!await _userManager.CheckPasswordAsync(member, adminVM.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Current Password Incorrect !");
                    return View("profile", profileVM);
                }
                var passwordResult = _userManager.ChangePasswordAsync(member, adminVM.CurrentPassword, adminVM.Password);
                if (!passwordResult.Result.Succeeded)
                {
                    foreach (var item in passwordResult.Result.Errors)
                    {
                        ModelState.AddModelError("Password", item.Description);
                    }
                    return View("profile", profileVM);
                }
            }
            TempData["Success"] = "Profile updated !";
            await _signInManager.SignInAsync(member, false);
            return RedirectToAction("profile");
        }
    }
}
