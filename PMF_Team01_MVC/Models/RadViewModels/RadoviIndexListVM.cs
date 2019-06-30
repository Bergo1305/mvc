using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PMF_Team01_MVC.Models.RadViewModels
{
    public class RadoviIndexListVM
    {
        public IEnumerable<RadoviIndexVM> Radovi { get; set; }
        public List<SelectListItem> TipoviRada { get; set; }
        public string SelectedTip { get; set; }
        public int PublikacijaId { get; set; }
    }
}
