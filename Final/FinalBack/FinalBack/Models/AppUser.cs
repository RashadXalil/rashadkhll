using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FinalBack.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(maximumLength:150)]
        public string FullName { get; set; }
        [StringLength(maximumLength: 150)]
        public string Name { get; set; }
        [StringLength(maximumLength: 150)]
        public string Surname { get; set; }
        public bool IsAdmin { get; set; }
        [StringLength(maximumLength: 150)]

        public string CompanyName { get; set; }
        [StringLength(maximumLength: 150)]

        public string PostCode { get; set; }
        [StringLength(maximumLength: 50)]

        public string Country { get; set; }
        [StringLength(maximumLength: 150)]

        public string Address { get; set; }
        [StringLength(maximumLength: 150)]

        public string Address2 { get; set; }
        [StringLength(maximumLength: 150)]

        public string City { get; set; }
        [StringLength(maximumLength: 50)]

        public string Phone { get; set; }

    }
}
