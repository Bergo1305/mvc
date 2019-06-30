using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models
{
    public class EKnjiga
    {
        [ForeignKey("Rad")]
        public int EKnjigaId { get; set; }
        public virtual Rad Rad { get; set; }
    }
}
