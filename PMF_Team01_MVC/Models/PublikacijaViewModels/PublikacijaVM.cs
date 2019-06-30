using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.PublikacijaViewModels
{
    public class PublikacijaVM
    {
        public int Id { get; set; } //nije obavezno, ali ce koristiti u Edit view modelu

        [Required]
        public string BrojPublikacije { get; set; }
        public string Napomena { get; set; }
        public DateTime? DatumIzdavanja { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public IEnumerable<RadViewModels.RadoviIndexVM> ListaRadova { get; set; }
    }

    public class PublikacijaListVM
    {
        public IEnumerable<PublikacijaVM> Publikacije { get; set; }
    }
}
