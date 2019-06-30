using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PMF_Team01_MVC.Models.PublikacijaViewModels
{
    public class PublikacijaSelectVM
    {
        public int radID { get; set; }
        public List<SelectListItem> Publikacije { get; set; }
        public string publikacijaID { get; set; }
    }
}
