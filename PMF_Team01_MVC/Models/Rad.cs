using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models
{
    public class Rad
    {
        [Key]
        public int Id { get; set; }

        public string Naslov { get; set; }
        public string Apstrakt { get; set; }
        public string OstaliAutori { get; set; }
        public string KeyWords { get; set; }
        public string Tip { get; set; }
        public bool? ApprovedByAdmin { get; set; }
        public DateTime? UploadDate { get; set; }
        public DateTime? DatumObjavljivanja { get; set; }
        public int BrojPozitivnihOcjena { get; set; } //broj pozitivnih ocjena od strane recenzenata

        [ForeignKey("Publikacija")]
        public int? PublikacijaId { get; set; }
        public virtual Publikacija Publikacija { get; set; }
    }
}
