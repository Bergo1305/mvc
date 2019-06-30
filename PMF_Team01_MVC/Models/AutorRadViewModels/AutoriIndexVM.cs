using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.AutorRadViewModels
{
    public class AutoriIndexVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Affiliation { get; set; }
    }

    public class AutoriIndexListVM
    {
        public IEnumerable<AutoriIndexVM> Autori { get; set; }
    }
}
