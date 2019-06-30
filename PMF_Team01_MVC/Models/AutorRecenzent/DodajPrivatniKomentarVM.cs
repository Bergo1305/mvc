using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PMF_Team01_MVC.Models.AutorRecenzent
{
    public class DodajPrivatniKomentarVM
    {
        [Required]
        public int RadId { get; set; }

        [Required]
        public string Sadrzaj { get; set; }

        [Required]
        public string AutorId { get; set; }

        public IFormFile Dokument { get; set; }

        public int IsAutor { get; set; }

        public int KomentarId { get; set; }
    }
}
