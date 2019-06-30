using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.RadViewModels
{
    public class DodajKomentarVM
    {
        public int RadId { get; set; }
        [MinLength(5)]
        [MaxLength(1000)]
        [Required]
        public string Sadrzaj { get; set; }
        public DateTime Datum { get; set; }

        public int KomentarId { get; set; }
    }
}
