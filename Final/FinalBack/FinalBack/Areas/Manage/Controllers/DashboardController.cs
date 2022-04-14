using FinalBack.Areas.Manage.ViewModels;
using FinalBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FinalBack.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]

    public class DashboardController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public DashboardController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
        public IActionResult Index()
        {
            List<Order> orders = _context.Orders.Include(x=>x.OrderItems).ThenInclude(x=>x.Product).ToList();
            List<OrderItem> orderItems= _context.OrderItems.Include(x => x.Product).ToList();
            List<Order> PendingOrders = _context.Orders.Include(x => x.OrderItems).Where(x => x.Status == Enums.OrderStatus.Pending).ToList();
            List<Order> AcceptedOrders = _context.Orders.Include(x => x.OrderItems).Where(x => x.Status == Enums.OrderStatus.Accepted).ToList();
            List<Order> RejectedOrders = _context.Orders.Include(x => x.OrderItems).Where(x => x.Status == Enums.OrderStatus.Rejected).ToList();
            List<Order> CanceledOrders = _context.Orders.Include(x => x.OrderItems).Where(x => x.Status == Enums.OrderStatus.Canceled).ToList();
            List<Order> OnCourrierOrders = _context.Orders.Include(x => x.OrderItems).Where(x => x.Status == Enums.OrderStatus.OnCourrier).ToList();  
            List<Order> DeliveredOrders = _context.Orders.Include(x => x.OrderItems).Where(x => x.Status == Enums.OrderStatus.Delivered).ToList();
            List<Product> GamingLaptops = _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.SubCategory).ThenInclude(x => x.Category).Where(x => x.SubCategory.Name == "Gaming Laptop").ToList();
            List<Product> UltraBooks = _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.SubCategory).ThenInclude(x => x.Category).Where(x => x.SubCategory.Name == "Ultrabook").ToList();
            List<Product> Phones = _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.SubCategory).ThenInclude(x => x.Category).Where(x => x.SubCategory.Name == "SmartPhones").ToList();
            List<Product> Tablets = _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.SubCategory).ThenInclude(x => x.Category).Where(x => x.SubCategory.Name == "Tablet").ToList();
            List<Product> Pcs = _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.SubCategory).ThenInclude(x => x.Category).Where(x => x.SubCategory.Name == "Gaming PC").ToList();
            List<Product> VideoCards = _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.SubCategory).ThenInclude(x => x.Category).Where(x => x.SubCategory.Name == "Video Card").ToList();
            List<Product> Watches = _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.SubCategory).ThenInclude(x => x.Category).Where(x => x.SubCategory.Name == "Watch").ToList();
            DashboardViewModel dashboardVM = new DashboardViewModel()
            {
                AllOrders = orders,
                OrderItems = orderItems,
                PendingOrders = PendingOrders,
                AcceptedOrders = AcceptedOrders,
                RejectedOrders = RejectedOrders,
                CanceledOrders = CanceledOrders,
                OnCourrierOrders = OnCourrierOrders,
                DeliveredOrders = DeliveredOrders,
                GamingLaptops = GamingLaptops,
                UltraBooks = UltraBooks,
                Phones = Phones,
                Tablets = Tablets,
                PCs = Pcs,
                VideoCards = VideoCards,
                Watches = Watches

            };
            return View(dashboardVM);
        }
    }
}
