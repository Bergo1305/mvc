using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.AdminUsersViewModels
{
    public class UserIndexVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Affiliation { get; set; }
        public bool? IsEnabled { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public string Username { get; set; }
    }
}
