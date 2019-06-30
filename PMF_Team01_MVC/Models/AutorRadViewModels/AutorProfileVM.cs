using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.AutorRadViewModels
{
    public class AutorProfileVM
    {
        public string Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Affiliation { get; set; }
        public string ImagePath { get; set; }
        public string Mjesto { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string PhoneNumber { get; set; }
        public string CVPath { get; set; }
    }
}
