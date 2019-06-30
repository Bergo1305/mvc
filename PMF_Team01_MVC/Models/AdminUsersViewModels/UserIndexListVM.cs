using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.AdminUsersViewModels
{
    public class UserIndexListVM
    {
        public IEnumerable<UserIndexVM> Users { get; set; }
    }
}
