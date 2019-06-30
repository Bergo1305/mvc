using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.RadViewModels
{
    public class RadDetailsVM
    {
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string TipRada { get; set; }
        public string Apstrakt { get; set; }
        public bool? IsOdobren { get; set; }
        public IEnumerable<string> Oblasti { get; set; }
        public string GlavniAutor { get; set; }
        public string OstaliAutori { get; set; }

        public string KeyWords { get; set; }
        public DateTime? DatumObjavljivanja { get; set; }

        //Recenzirani
        public string TipRecenziranogRada { get; set; }

        //Idea
        public string TekstIdeje { get; set; }
        public List<string> Kategorije { get; set; }

        //Studentski Rad
        public string Mentor { get; set; }
        public string TipStudentskogRada { get; set; }
        public string Napomena { get; set; }

        //EKnjiga 
        //Lista stringova koja cuva path do svake recenzije
        public List<string> recenzije { get; set; }

        public bool DocumentExists { get; set; }
    }
}
