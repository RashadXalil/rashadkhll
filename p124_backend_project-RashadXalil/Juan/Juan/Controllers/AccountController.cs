using Juan.Models;
using Juan.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly JuanContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(JuanContext context,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult MemberLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> MemberLogin(MemberLoginViewModel LoginVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser member = await _userManager.Users.FirstOrDefaultAsync(x=>x.UserName == LoginVM.UserName && !x.IsAdmin); 

            if(member == null)
            {
                ModelState.AddModelError("", "Username or password is incorrect ! ");
                return View();
            }


            var result = await _signInManager.PasswordSignInAsync(member, LoginVM.Password, LoginVM.isPersistent,false);

            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password incorrect ! ");
            }

            return RedirectToAction("index","home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterViewModel registerVM)
        {
            
            if (!ModelState.IsValid) return View();

            AppUser member = await _userManager.FindByNameAsync(registerVM.UserName);

            if (member!=null)
            {
                ModelState.AddModelError("Username", "Username already exist !");
                return View();
            }

            member = new AppUser
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                IsAdmin = false,
            };

            var result = await _userManager.CreateAsync(member,registerVM.Password);

            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                return View();
            }

            var roleResult = await _userManager.AddToRoleAsync(member, "Member");

            if(!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }


            await _signInManager.SignInAsync(member, true);

            return RedirectToAction("index","home");
        }

        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        [Authorize(Roles ="Member")] 
        public async Task<IActionResult> Profile()
        {
            AppUser member = await _userManager.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            if (member is null) return PartialView("_CustomErrorPartial");

            MemberProfileViewModel profileVM = new MemberProfileViewModel
            {
                Member = new MemberUpdateViewModel
                {
                    FullName = member.FullName,
                    Adress = member.Address,
                    City = member.City,
                    Country = member.Country,
                    Email = member.Email,
                    Phonenumber = member.PhoneNumber,
                    UserName = member.UserName
                },
                Orders = _context.Orders.Include(x=>x.OrderItems).ThenInclude(x=>x.Product).Where(x=>x.AppUserId==member.Id).ToList()
            };
            return View(profileVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(MemberUpdateViewModel memberVM)
        {
            if (memberVM == null) return NotFound();
            MemberProfileViewModel profileVM = new MemberProfileViewModel
            {
                Member = memberVM
            };


            if (!ModelState.IsValid) 
                return View("profile", profileVM);



            AppUser member = await _userManager.FindByNameAsync(User.Identity.Name);


            if(member.UserName != memberVM.UserName)
            {
                if (_userManager.Users.Any(x =>x.NormalizedUserName == memberVM.UserName.ToUpper()))
                {
                    ModelState.AddModelError("Username", "Username already exist");
                    return View("profile", profileVM);
                }
            }
            if(member.Email != memberVM.Email)
            {
            if (_userManager.Users.Any(x=>x.NormalizedEmail == memberVM.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "Email already exist");
                return View("profile", profileVM);
            }
            }
           

            member.Email = memberVM.Email;
            member.FullName = memberVM.FullName;
            member.Country = memberVM.Country;
            member.City = memberVM.City;
            member.Address = memberVM.Adress;
            member.PhoneNumber = memberVM.Phonenumber;
            member.UserName = memberVM.UserName;

            var result =await _userManager.UpdateAsync(member);
            if(memberVM.Password!=null)
            {
                if(string.IsNullOrWhiteSpace(memberVM.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Current Password required !");
                    return View("profile", profileVM);
                }

                if(! await _userManager.CheckPasswordAsync(member,memberVM.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Current Password Incorrect !");
                    return View("profile",profileVM);
                }
                var passwordResult = _userManager.ChangePasswordAsync(member, memberVM.CurrentPassword,memberVM.Password);
                if(! passwordResult.Result.Succeeded)
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
