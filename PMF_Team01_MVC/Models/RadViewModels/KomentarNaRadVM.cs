using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.RadViewModels
{
    public class KomentarNaRadVM
    {
        public int Id { get; set; }
        public int RadId { get; set; }
        public string Autor { get; set; }
        public string Datum { get; set; }
        public string Sadrzaj { get; set; }
    }

    public class KomentarNaRadIndexVM
    {
        public int RadId { get; set; }
        public string NaslovRada { get; set; }
        public IEnumerable<KomentarNaRadVM> Komentari { get; set; }
    }
}
