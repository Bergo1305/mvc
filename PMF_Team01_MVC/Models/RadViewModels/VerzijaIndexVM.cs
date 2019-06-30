using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.RadViewModels
{
    public class VerzijaIndexVM
    {
        public int VerzijaId { get; set; }
        public string FileName { get; set; }
        public string Verzija { get; set; }
        public bool TrenutnaVerzija { get; set; }
    }

    public class VerzijaIndexListVM
    {
        public int RadId { get; set; }
        public string NazivRada { get; set; }
        public IEnumerable<VerzijaIndexVM> Verzije { get; set; }
    }
}
