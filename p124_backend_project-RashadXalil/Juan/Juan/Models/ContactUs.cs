using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class ContactUs
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:40)]
        public string FullName { get; set; }
        [Required]
        [StringLength(maximumLength: 80)]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 40)]
        public string PhoneNumber { get; set; }
        [StringLength(maximumLength: 20)]
        public string Subject { get; set; }
        [Required]
        [StringLength(maximumLength: 150)]
        public string Message { get; set; }
    }
}
