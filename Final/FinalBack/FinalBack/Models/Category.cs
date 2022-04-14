using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalBack.Models
{
    public class Category:BaseEntity
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }
        public string Image { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
