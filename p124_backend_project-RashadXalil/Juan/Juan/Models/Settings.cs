using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Settings
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string Key { get; set; }
        [Required]
        [StringLength(maximumLength: 500)]
        public string Value { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
