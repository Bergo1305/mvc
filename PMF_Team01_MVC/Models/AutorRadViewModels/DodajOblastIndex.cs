using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.AutorRadViewModels
{
    public class DodajOblastIndex
    {
        public int RadId { get; set; }

        public string RecenzentId { get; set; }

        public List<OblastIndexVM> Oblasti { get; set; }

        public List<OblastIndexVM> OblastiRada { get; set; }

    }
}
