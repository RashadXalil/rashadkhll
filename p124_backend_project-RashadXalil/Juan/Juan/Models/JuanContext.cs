using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class JuanContext : IdentityDbContext
    {
        public JuanContext(DbContextOptions<JuanContext> options) : base(options)
        {

        }
        public DbSet<Features> Features { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ContactUs> ContactUses { get; set; }

    }
}
