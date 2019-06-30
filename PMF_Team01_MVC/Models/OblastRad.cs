using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models
{
    public class OblastRad
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Rad")]
        public int RadId { get; set; }
        public virtual Rad Rad { get; set; }

        [ForeignKey("Oblast")]
        public int OblastId { get; set; }
        public virtual Oblast Oblast { get; set; }
    }
}
