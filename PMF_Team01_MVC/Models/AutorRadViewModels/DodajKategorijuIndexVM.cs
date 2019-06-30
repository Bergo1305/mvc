using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.AutorRadViewModels
{
    public class KategorijaIndexVM
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
    }

    public class DodajKategorijuIndexVM
    {
        public int IdejaId { get; set; }
        public List<KategorijaIndexVM> KategorijeIdeje { get; set; }
        public List<KategorijaIndexVM> Kategorije { get; set; }
    }
}
