using FinalBack.Models;
using FinalBack.Utils;
using FinalBack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FinalBack.Controllers
{
    public class AccountController : Controller
    {
        private readonly FinalContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public AccountController(FinalContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
        }
        public IActionResult MemberLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> MemberLogin(MemberLoginViewModel LoginVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser member = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == LoginVM.UserName && !x.IsAdmin);

            if (member == null)
            {
                ModelState.AddModelError("", "Username or password is incorrect ! ");
                return View();
            }


            var result = await _signInManager.PasswordSignInAsync(member, LoginVM.Password, LoginVM.isPersistent, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password incorrect ! ");
            }

            return RedirectToAction("index", "home");
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
            AppUser memberemail = await _userManager.FindByEmailAsync(registerVM.Email);
            if (member != null)
            {
                ModelState.AddModelError("Username", "Username already exist !");
                return View();
            }
            if (memberemail != null)
            {
                ModelState.AddModelError("Email", "Email already exist !");
                return View();
            }

            member = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                FullName = registerVM.Name +" " +registerVM.Surname,
                IsAdmin = false,
            };

            var result = await _userManager.CreateAsync(member, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            var roleResult = await _userManager.AddToRoleAsync(member, "Member");

            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(member.Email);
            mailMessage.From = new MailAddress("rashadmkh@code.edu.az");
            mailMessage.Subject = "Hello Dear Customer";
            string emailbody = string.Empty;
            using (StreamReader streamReader = new StreamReader(Path.Combine(_env.WebRootPath, "templates", "Register.html")))
            {
                emailbody = streamReader.ReadToEnd();
            }
            emailbody = emailbody.Replace("{{CustomerName}}", member.Name.ToUpper() + " " + member.Surname.ToUpper());
            emailbody = emailbody.Replace("{{Welcome}}", member.Name.ToUpper() + " " + member.Surname.ToUpper());
            mailMessage.Body = emailbody;
            mailMessage.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential("rashadmkh@code.edu.az", "rashad918");
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.Send(mailMessage);
            await _signInManager.SignInAsync(member, true);
            TempData["Success"] = "Succesfully Registred !";

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile()
        {
            AppUser member = await _userManager.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            if (member is null) return RedirectToAction("Error","Home");

            MemberProfileViewModel profileVM = new MemberProfileViewModel
            {
                Member = new MemberUpdateViewModel
                {
                    FirstName = member.Name,
                    Surname = member.Surname,
                    Adress = member.Address,
                    City = member.City,
                    Country = member.Country,
                    Email = member.Email,
                    Phonenumber = member.PhoneNumber,
                    UserName = member.UserName
                },
                Orders = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).ThenInclude(x=>x.Brand).Where(x => x.AppUserId == member.Id).ToList()
            };
            return View(profileVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(MemberUpdateViewModel memberVM)
        {
            AppUser member = await _userManager.FindByNameAsync(User.Identity.Name);

            if (memberVM == null) return NotFound();

            MemberProfileViewModel profileVM = new MemberProfileViewModel
            {
                Member = memberVM,
                Orders = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).ThenInclude(x => x.Brand).Where(x => x.AppUserId == member.Id).ToList()
            };
            if (!ModelState.IsValid)
                return View("profile", profileVM);

            if (member.UserName != memberVM.UserName)
            {
                if (_userManager.Users.Any(x => x.NormalizedUserName == memberVM.UserName.ToUpper()))
                {
                    ModelState.AddModelError("Username", "Username already exist");
                    return View("profile", profileVM);
                }
            }
            if (member.Email != memberVM.Email)
            {
                if (_userManager.Users.Any(x => x.NormalizedEmail == memberVM.Email.ToUpper()))
                {
                    ModelState.AddModelError("Email", "Email already exist");
                    return View("profile", profileVM);
                }
            }


            member.Email = memberVM.Email;
            member.FullName = memberVM.FirstName;
            member.Surname = memberVM.Surname;
            member.Country = memberVM.Country;
            member.City = memberVM.City;
            member.Address = memberVM.Adress;
            member.PhoneNumber = memberVM.Phonenumber;
            member.UserName = memberVM.UserName;

            var result = await _userManager.UpdateAsync(member);
            if (memberVM.Password != null)
            {
                if (string.IsNullOrWhiteSpace(memberVM.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Current Password required !");
                    return View("profile", profileVM);
                }

                if (!await _userManager.CheckPasswordAsync(member, memberVM.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Current Password Incorrect !");
                    return View("profile", profileVM);
                }
                var passwordResult = _userManager.ChangePasswordAsync(member, memberVM.CurrentPassword, memberVM.Password);
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
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();

            var dbUser = await _userManager.FindByEmailAsync(email);
            if (dbUser is null)
                return NotFound();


            var token = await _userManager.GeneratePasswordResetTokenAsync(dbUser);

            var link = Url.Action("ResetPassword", "Account", new { dbUser.Id, token }, protocol: HttpContext.Request.Scheme);

            var message = $"<a href='{link}'>reset password</a>";

            await EmailUtil.SendEmailAsync(email, "Reset Password", message);

            return RedirectToAction("MemberLogin");
        }
        public async Task<IActionResult> ResetPassword(string id, string token)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(token))
                return BadRequest();

            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser is null)
                return NotFound();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, string token, ResetPasswordViewModel resetPasswordVm)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(token))
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser is null)
                return NotFound();

            var result = await _userManager.ResetPasswordAsync(dbUser, token, resetPasswordVm.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            return RedirectToAction("MemberLogin");
        }

    }
}
