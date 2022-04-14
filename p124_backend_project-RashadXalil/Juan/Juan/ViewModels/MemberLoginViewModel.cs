using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels
{
    public class MemberLoginViewModel
    {
        [Required]
        [StringLength(maximumLength: 40)]
        public string UserName { get; set; }
        [Required]
        [StringLength(maximumLength: 25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool isPersistent { get; set; }

    }
}
