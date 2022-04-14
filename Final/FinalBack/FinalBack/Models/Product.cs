using Castle.MicroKernel.SubSystems.Conversion;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalBack.Models
{
    public class Product:BaseEntity
    {
        [Required]
        [StringLength(maximumLength:100)]
        public string Name { get; set; }
        [StringLength(maximumLength: 200)]
        public string Desc { get; set; }
        [StringLength(maximumLength: 200)]
        public string SoundSystem { get; set; }
        [StringLength(maximumLength: 200)]
        public string OperationSystem { get; set; }

        public int BrandId { get; set; }
        public int SubCategoryId { get; set; }
        public int Rate { get; set; }
        public bool IsNew { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsTrending { get; set; }
        public int Ram { get; set; }
        public int Memory { get; set; }
        public double ScreenSize { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercent { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Weight { get; set; }
        public int ColorId { get; set; }
        [NotMapped]
        public List<int> FileIds { get; set; } = new List<int>();
        [NotMapped] 
        public IFormFile ThumbFile { get; set; }
        [NotMapped] 
        public List<IFormFile> Files { get; set; }
        public Brand Brand { get; set; }
        public Color Color { get; set; }
        public SubCategory SubCategory { get; set; }
        public List<Review> Reviews { get; set; }
        public List<ProductImage> ProductImages { get; set; }

    }
}
