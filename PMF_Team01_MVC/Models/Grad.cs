using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models
{
    public class Grad
    {
        [Key]
        public int Id { get; set; }

        public string Naziv { get; set; }
        
        //[ForeignKey("Drzava")]
        //public int DrzavaId { get; set; }

        //public virtual Drzava Drzava { get; set; }
    }
}
