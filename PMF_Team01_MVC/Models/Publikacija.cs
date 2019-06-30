using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models
{
    public class Publikacija
    {
        [Key]
        public int Id { get; set; }

        public string BrojPublikacije { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public DateTime? DatumIzdavanja { get; set; }
        public string Napomena { get; set; }
    }
}
