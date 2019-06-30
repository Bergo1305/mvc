using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.RecenzentViewModels
{
    public class EditRadRecenzentVM
    {
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string Apstrakt { get; set; }
        public string TipRada { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool? ApprovedByAdmin { get; set; }
        public bool? MojaOcjena { get; set; }
        public string KeyWords { get; set; }
        public DateTime? UploadDate { get; set; }
        public List<string> Oblasti { get; set; }

        //Recenzirani Rad
        public string TipRecenziranogRada { get; set; }

        //Ideja
        public string TekstIdeje { get; set; }
        public List<string> Kategorije { get; set; }

        public bool DocumentExists { get; set; }
    }
}
