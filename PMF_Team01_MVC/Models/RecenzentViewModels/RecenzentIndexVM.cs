using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMF_Team01_MVC.Models.AutorRadViewModels;

namespace PMF_Team01_MVC.Models.RecenzentViewModels
{
    public class RecenzentIndexVM
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<string> Oblasti { get; set; }
    }
}
