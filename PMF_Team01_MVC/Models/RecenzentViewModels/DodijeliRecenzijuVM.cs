using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.RecenzentViewModels
{
    public class DodijeliRecenzijuVM
    {
        public int RecenzijaId { get; set; }
        public string Autor { get; set; }
    }

    public class DodijeliRecenzijuIndexVM
    {
        public int EKnjigaId { get; set; }
        public IEnumerable<DodijeliRecenzijuVM> Recenzije { get; set; }
    }
}
