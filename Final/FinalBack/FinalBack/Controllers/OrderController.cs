using FinalBack.Models;
using FinalBack.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FinalBack.Controllers
{
    public class OrderController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;
        public OrderController(UserManager<AppUser> userManager, FinalContext context,IWebHostEnvironment env)
        {
            _userManager = userManager;
            _context = context;
            _env = env;
        }
        public IActionResult Checkout()
        {
            AppUser member = null;
            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }

            CheckOutViewModel checkoutVM = new CheckOutViewModel
            {
                Basket = _getBasket(member),
                Order = new Order
                {
                    Email = member?.Email,
                    PhoneNumber = member?.PhoneNumber,
                    FirstName = member?.Name,
                    Surname = member?.Surname,
                    Address = member?.Address,
                    City = member?.City,
                    Country = member?.Country,
                }
            };
            return View(checkoutVM);
        }
        public BasketViewModel _getBasket(AppUser member)
        {
            BasketViewModel basketVM = new BasketViewModel
            {
                BasketProducts = new List<ProductBasketItemViewModel>()
            };

            List<BasketItemViewModel> items = new List<BasketItemViewModel>();

            if (member == null)
            {
                string basketItemsStr = HttpContext.Request.Cookies["basket"];

                if (!string.IsNullOrWhiteSpace(basketItemsStr))
                    items = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);
            }
            else
            {
                items = _context.BasketItems.Where(x => x.AppUserId == member.Id).Select(b => new BasketItemViewModel { ProductId = b.ProductId, Count = b.Count }).ToList();
            }
            foreach (var item in items)
            {
                Product product = _context.Products.Include(x => x.Brand).Include(x => x.SubCategory).ThenInclude(x=>x.Category).Include(x=>x.Color).Include(x=>x.ProductImages).FirstOrDefault(x => x.Id == item.ProductId);
                ProductBasketItemViewModel bookItem = new ProductBasketItemViewModel
                {
                    Product = product,
                    Count = item.Count
                };

                decimal totalPrice = product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice;
                basketVM.TotalPrice += totalPrice * item.Count;

                basketVM.BasketProducts.Add(bookItem);
            }
            return basketVM;
        }
        [HttpPost]
        public IActionResult Create(Order order)
        {
            AppUser member = null;
            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }

            CheckOutViewModel checkoutVM = new CheckOutViewModel
            {
                Basket = _getBasket(member),
                Order = order
            };

            if (!ModelState.IsValid)
                return View("Checkout", checkoutVM);

            if (checkoutVM.Basket.BasketProducts.Count == 0)
            {
                ModelState.AddModelError("", "You must chose any product!");
                return View("Checkout", checkoutVM);
            }
            Guid guid = Guid.NewGuid();

            order.AppUserId = member?.Id;
            order.CreatedAt = DateTime.UtcNow.AddHours(4);
            order.Status = Enums.OrderStatus.Pending;
            order.OrderItems = new List<OrderItem>();
            order.TrackId = $"{guid}";
            foreach (var item in checkoutVM.Basket.BasketProducts)
            {
                OrderItem orderItem = new OrderItem
                {
                    ProductId = item.Product.Id,
                    SalePrice = item.Product.SalePrice,
                    CostPrice = item.Product.CostPrice,
                    DiscountedPrice = item.Product.DiscountPercent > 0 ? (item.Product.SalePrice * (1 - item.Product.DiscountPercent / 100)) : item.Product.SalePrice,
                    Count = item.Count
                };      
                
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(order.Email);
                mailMessage.From = new MailAddress("rashadmkh@code.edu.az");
                mailMessage.Subject = "Hello Dear Customer";
                string emailbody = string.Empty;
                using (StreamReader streamReader = new StreamReader(Path.Combine(_env.WebRootPath, "templates", "Track.html")))
                {
                    emailbody = streamReader.ReadToEnd();
                }
                emailbody = emailbody.Replace("{{SifarisId}}", order.TrackId);
                emailbody = emailbody.Replace("{{CustomerName}}", order.FirstName.ToUpper() + " " +order.Surname.ToUpper());
                string mailItem = string.Empty;
                mailMessage.Body = emailbody;
                mailMessage.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential("rashadmkh@code.edu.az", "rashad918");
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                smtp.Send(mailMessage);

                order.OrderItems.Add(orderItem);
                order.TotalPrice += orderItem.DiscountedPrice * orderItem.Count;
            }

            _context.Orders.Add(order);

            if (member == null)
                HttpContext.Response.Cookies.Delete("basket");
            else
                _context.BasketItems.RemoveRange(_context.BasketItems.Where(x => x.AppUserId == member.Id));
            TempData["Success"] = "Order Created !";

            _context.SaveChanges();

            return RedirectToAction("index", "home");
        }
        public IActionResult Card()
        {
            AppUser member = null;

            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }

            if (member == null)
            {
                return RedirectToAction("register", "account");
            }
            else
            {
                List<BasketItem> items = _context.BasketItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.AppUserId == member.Id).ToList();
                return View(items);
            }
        }
        public IActionResult AddCard(int id)
        {
            if (!_context.Products.Any(x => x.Id == id))
            {
                return NotFound();
            }
            AppUser member = null;
            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }
            if (member == null)
            {
                string productIdsStr = HttpContext.Request.Cookies["basket"];
                List<BasketItemViewModel> items = new List<BasketItemViewModel>();
                if (!string.IsNullOrWhiteSpace(productIdsStr))
                {
                    items = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(productIdsStr);
                }
                BasketItemViewModel item = items.FirstOrDefault(x => x.ProductId == id);
                if (item == null)
                {
                    item = new BasketItemViewModel { ProductId = id, Count = 1 };
                    items.Add(item);
                }
                else
                {
                    item.Count++;
                }
                productIdsStr = JsonConvert.SerializeObject(items);
                HttpContext.Response.Cookies.Append("basket", productIdsStr);
                BasketViewModel basketVM = new BasketViewModel
                {
                    BasketProducts = new List<ProductBasketItemViewModel>(),

                    TotalPrice = 0
                };
                foreach (var item1 in items)
                {
                    Product product = _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.SubCategory).Include(x => x.SubCategory).Include(x => x.Color).FirstOrDefault(x => x.Id == item.ProductId);
                    ProductBasketItemViewModel productBasketItem = new ProductBasketItemViewModel
                    {
                        Product = product,
                        Count = item1.Count
                    };

                    basketVM.BasketProducts.Add(productBasketItem);
                    decimal totalPrice = product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice;
                    basketVM.TotalPrice += totalPrice * item.Count;
                }
                TempData["Success"] = "Product Added to Card !";
                return RedirectToAction("card", "order", basketVM);
            }
            else
            {
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.AppUserId == member.Id && x.ProductId == id);

                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        AppUserId = member.Id,
                        ProductId = id,
                        CreatedAt = DateTime.UtcNow.AddHours(4),
                        Count = 1,
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }
                _context.SaveChanges();
                List<BasketItem> items = _context.BasketItems.Where(x => x.AppUserId == member.Id).ToList();

                BasketViewModel basketVM = new BasketViewModel
                {
                    BasketProducts = new List<ProductBasketItemViewModel>(),
                    TotalPrice = 0
                };

                foreach (var item in items)
                {
                    Product product = _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.SubCategory).ThenInclude(x => x.Category).FirstOrDefault(x => x.Id == item.ProductId);
                    ProductBasketItemViewModel productBasketItem = new ProductBasketItemViewModel
                    {
                        Product = product,
                        Count = item.Count
                    };

                    basketVM.BasketProducts.Add(productBasketItem);
                    decimal totalPrice = product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice;
                    basketVM.TotalPrice += totalPrice * item.Count;
                }
                TempData["Success"] = "Product Added to Card !";
                return RedirectToAction("card", basketVM);
            }
        }
        private BasketViewModel _getBasket(List<BasketItemViewModel> basketItems)
        {
            BasketViewModel basketVM = new BasketViewModel
            {
                BasketProducts = new List<ProductBasketItemViewModel>(),
                TotalPrice = 0
            };

            foreach (var item in basketItems)
            {
                Product product = _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.SubCategory).Include(x => x.SubCategory).Include(x => x.Color).FirstOrDefault(x => x.Id == item.ProductId);
                ProductBasketItemViewModel productBasketItem = new ProductBasketItemViewModel
                {
                    Product = product,
                    Count = item.Count
                };

                basketVM.BasketProducts.Add(productBasketItem);
                decimal totalPrice = product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice;
                basketVM.TotalPrice += totalPrice * item.Count;
            }

            return basketVM;
        }
        private BasketViewModel _getBasket(List<BasketItem> basketItems)
        {
            BasketViewModel basketVM = new BasketViewModel
            {
                BasketProducts = new List<ProductBasketItemViewModel>(),
                TotalPrice = 0
            };

            foreach (var item in basketItems)
            {
                Product product = _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.SubCategory).ThenInclude(x => x.Category).FirstOrDefault(x => x.Id == item.ProductId);
                ProductBasketItemViewModel productBasketItem = new ProductBasketItemViewModel
                {
                    Product = product,
                    Count = item.Count
                };

                basketVM.BasketProducts.Add(productBasketItem);
                decimal totalPrice = product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice;
                basketVM.TotalPrice += totalPrice * item.Count;
            }

            return basketVM;
        }
    }
}
