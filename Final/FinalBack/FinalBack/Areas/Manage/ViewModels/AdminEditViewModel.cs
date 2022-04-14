using FinalBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalBack.Areas.Manage.ViewModels
{
    public class AdminEditViewModel
    {
        public string AdminId { get; set; }
        public string CurrentRole { get; set; }
        public string NewRole { get; set; }
    }
}
