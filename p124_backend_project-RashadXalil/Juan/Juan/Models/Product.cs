using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{

    public class Product
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public int GenderId { get; set; }
        public int CategoryId { get; set; }
        [StringLength(maximumLength: 150)]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercent { get; set; }
        public bool IsAvailable { get; set; }
        [StringLength(maximumLength: 150)]
        public string Image { get; set; }
        [StringLength(maximumLength: 200)]
        public string Desc { get; set; }
        [Range(0, 5)]
        public int Rate { get; set; }
        [NotMapped]
        public List<int> ColorIds { get; set; } = new List<int>();
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public Gender Gender { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public List<ProductColor> productColors { get; set; }
        public List<Review> Reviews { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }
}
