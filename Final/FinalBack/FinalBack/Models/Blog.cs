using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalBack.Models
{
    public class Blog:BaseEntity
    {
        public int View { get; set; }
        [StringLength(maximumLength: 400)]
        public string Desc1 { get; set; }
        public string Image { get; set; }
        [StringLength(maximumLength: 400)]
        public string Title1 { get; set; }
        [StringLength(maximumLength: 400)]
        public string Desc2 { get; set; }
        [StringLength(maximumLength: 400)]
        public string Title2 { get; set; }
        [StringLength(maximumLength: 400)]
        public string Desc3 { get; set; }
        public bool IsPopular { get; set; }
        public int BlogCategoryId { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public BlogCategory BlogCategory { get; set; }
    }
}
