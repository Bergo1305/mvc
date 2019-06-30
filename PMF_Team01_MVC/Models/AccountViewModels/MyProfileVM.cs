using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.AccountViewModels
{
    public class MyProfileVM
    {
        public string Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string Affiliation { get; set; }
        public string Initials { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string CVPath { get; set; }
        public List<string> Roles { get; set; }
        public string Grad { get; set; }
        public string State { get; set; }
        public string Drzava { get; set; }
    }
}
