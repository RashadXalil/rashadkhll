using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels
{
    public class MemberUpdateViewModel
    {
        [Required]
        [StringLength(maximumLength: 40)]
        public string UserName { get; set; }
        [Required]
        [StringLength(maximumLength: 80)]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string FullName { get; set; }
        [StringLength(maximumLength: 50)]
        public string Country { get; set; }
        [StringLength(maximumLength: 50)]
        public string City { get; set; }
        [StringLength(maximumLength: 350)]
        public string Adress { get; set; }
        [StringLength(maximumLength:25)]
        [DataType(DataType.Password)]
        [Compare(nameof(ConfirmPassword))]
        public string Password { get; set; }
        [StringLength(maximumLength: 25)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [StringLength(maximumLength: 25)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        public string Phonenumber { get; set; }
    }
}
