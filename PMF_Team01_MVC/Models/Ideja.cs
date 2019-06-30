using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models
{
    public class Ideja
    {
        [ForeignKey("Rad")]
        public int IdejaId { get; set; }
        public virtual Rad Rad { get; set; }

        public string TekstIdeje { get; set; }
    }
}
