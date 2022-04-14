using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Features
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 150)]
        public string Image { get; set; }
        [Required]
        [StringLength(maximumLength: 150)]
        public string Title { get; set; }
        [Required]
        [StringLength(maximumLength: 150)]
        public string Desc { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
