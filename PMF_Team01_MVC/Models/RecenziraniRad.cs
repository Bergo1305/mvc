using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models
{
    public class RecenziraniRad
    {
        [ForeignKey("Rad")]
        public int RecenziraniRadId { get; set; }

        public string TipRecenziranogRada { get; set; } //rad za konferenciju i...

        public virtual Rad Rad { get; set; }
    }
}
