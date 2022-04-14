using System.ComponentModel.DataAnnotations;

namespace FinalBack.ViewModels
{
    public class MemberRegisterViewModel
    {
        [Required]
        [StringLength(maximumLength: 40)]
        public string UserName { get; set; }
        [Required]
        [StringLength(maximumLength: 80)]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string Surname { get; set; }
        [Required]
        [StringLength(maximumLength: 25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [StringLength(maximumLength: 25)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
