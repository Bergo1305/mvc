using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.AutorRecenzent
{
    public class PrivatniKomentariIndexVM
    {
        public int KomentarId { get; set; }
        public string AutorId { get; set; }
        public string Autor { get; set; }
        public string Sadrzaj { get; set; }
        public string FilePath { get; set; } //path do prilozenog fajla
    }

    public class PrivatniKomentariIndexListVM
    {
        public int RadId { get; set; }
        public int IsAuthor { get; set; }
        public IEnumerable<PrivatniKomentariIndexVM> Komentari { get; set; }
    }
}
