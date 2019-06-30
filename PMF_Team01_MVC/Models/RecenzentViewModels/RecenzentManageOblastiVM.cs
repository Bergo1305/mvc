using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMF_Team01_MVC.Models.AutorRadViewModels;

namespace PMF_Team01_MVC.Models.RecenzentViewModels
{
    public class RecenzentManageOblastiVM
    {
        public string RecenzentId { get; set; }
        public string RecenzentName { get; set; }

        public List<OblastIndexVM> SlobodneOblasti { get; set; }
        public List<OblastIndexVM> RecenzentOblasti { get; set; }
    }
}
