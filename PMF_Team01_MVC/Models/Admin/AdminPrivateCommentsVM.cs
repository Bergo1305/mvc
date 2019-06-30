using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.Admin
{
    public class AdminPrivateCommentsVM
    {
        public int KomentarId { get; set; }
        public string AutorId { get; set; }
        public string Autor { get; set; }
        public string Sadrzaj { get; set; }
        public string FilePath { get; set; } //path do prilozenog fajla
    }

    public class AdminPrivateCommentsListVM
    {
        public int RadId { get; set; }
        public IEnumerable<AdminPrivateCommentsVM> Komentari { get; set; }
    }
}
