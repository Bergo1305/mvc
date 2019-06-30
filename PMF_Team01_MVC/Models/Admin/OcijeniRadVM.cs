using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PMF_Team01_MVC.Models.Admin
{
    public class OcijeniRadVM
    {
        public int RadId { get; set; }
        public string Naslov { get; set; }
        public string Ocjena { get; set; }
        public List<SelectListItem> Ocjene { get; set; }
    }
}
