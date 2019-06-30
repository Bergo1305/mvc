using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.RadViewModels
{
    public class RadoviIndexVM
    {
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string Autor { get; set; }
        public bool? OcjenaAdmina { get; set; }
        public bool? OcjenaRecenzenta { get; set; }
        public int BrojPozitivnihOcjena { get; set; }
        public string TipRada { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? UploadDate { get; set; }
    }
}
