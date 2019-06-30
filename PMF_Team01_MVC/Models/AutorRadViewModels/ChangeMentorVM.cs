using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PMF_Team01_MVC.Models.AutorRadViewModels
{
    public class ChangeMentorVM
    {
        public int RadId { get; set; }
        public string Mentor { get; set; }
        public List<SelectListItem> MentorList { get; set; }
    }
}
