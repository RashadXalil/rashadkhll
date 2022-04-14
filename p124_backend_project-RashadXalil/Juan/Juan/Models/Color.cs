using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Color
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string HexCode { get; set; }
        public List<ProductColor> Colors { get; set; }


    }
}
