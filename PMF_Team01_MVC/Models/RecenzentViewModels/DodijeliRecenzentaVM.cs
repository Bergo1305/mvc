using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMF_Team01_MVC.Models.AdminUsersViewModels;
using PMF_Team01_MVC.Models.RecenzentViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PMF_Team01_MVC.Models.ReviewerViewModels
{
    public class DodijeliRecenzentaVM
    {
        public int RadId { get; set; }
        public string NazivRada { get; set; }
        public IEnumerable<RecenzentIndexVM> Recenzenti { get; set; }
        public IEnumerable<RecenzentIndexVM> DodijeljeniRecenzenti { get; set; }
    }
}
