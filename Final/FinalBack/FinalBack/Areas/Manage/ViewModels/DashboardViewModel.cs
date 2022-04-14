using FinalBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalBack.Areas.Manage.ViewModels
{
    public class DashboardViewModel
    {
        public List<Order> AllOrders { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<Order> PendingOrders { get; set; }
        public List<Order> AcceptedOrders { get; set; }
        public List<Order> RejectedOrders { get; set; }
        public List<Order> CanceledOrders { get; set; }
        public List<Order> OnCourrierOrders { get; set; }
        public List<Order> DeliveredOrders { get; set; }
        public decimal TotalEarning { get; set; }
        public decimal TotalSale { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public List<Product> Phones { get; set; }
        public List<Product> Tablets { get; set; }
        public List<Product> GamingLaptops { get; set; }
        public List<Product> UltraBooks { get; set; }
        public List<Product> PCs { get; set; }
        public List<Product> VideoCards { get; set; }
        public List<Product> Watches { get; set; }
    }
}
