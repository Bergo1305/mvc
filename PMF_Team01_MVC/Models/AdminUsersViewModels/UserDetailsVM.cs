using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.AdminUsersViewModels
{
    public class UserDetailsVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Initials { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ImagePath { get; set; }
        public string CVPath { get; set; }
        public string Affiliation { get; set; }
        public List<string> Roles { get; set; }
        public string Grad { get; set; }
        public string Drzava { get; set; }
    }
}
