using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Identity.Areas.Identity.ViewModels
{
    public class UserListViewModel
    {
        public List<userViewModel> Users { get; set; }
        public userViewModel User { get; set; }
    }
}
