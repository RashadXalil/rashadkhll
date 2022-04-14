using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalBack.Models
{
    public class Service : BaseEntity
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string Image { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string Title { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Desc { get; set; }
    }
}
