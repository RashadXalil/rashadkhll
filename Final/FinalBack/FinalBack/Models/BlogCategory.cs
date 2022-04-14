using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalBack.Models
{
    public class BlogCategory:BaseEntity
    {
        [StringLength(maximumLength: 200)]
        public string Name { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
