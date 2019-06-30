using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.Admin
{
    public class AdminRadDetails
    {
        public int RadId { get; set; }
        public string Naziv { get; set; }
        public string TipRada { get; set; }
        public string Apstrakt { get; set; }
        public DateTime? UploadDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public int BrojPozitivnihOcjena { get; set; }
        public bool? ApprovedByAdmin { get; set; }
        public string KeyWords { get;  set; }
        public string TipRecenziranogRada { get; set; }
        public List<string> Oblasti { get; set; }
        public string GlavniAutor { get; set; }
        public string OstaliAutori { get; set; }

        public string TipStudentskogRada { get; set; }
        public string Mentor { get; set; }

        public string TekstIdeje { get; set; }
        public List<string> Kategorije { get; set; }


        //lista stringova koja cuva path do svake recenzije
        //koristi se samo ako je tip rada eKnjiga
        public List<string> recenzije { get; set; }

        public bool DocumentExists { get; set; }
    }
}
