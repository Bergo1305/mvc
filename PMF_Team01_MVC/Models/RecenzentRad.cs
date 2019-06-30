using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models
{
    public class RecenzentRad
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string RecenzentId { get; set; }
        public virtual ApplicationUser Recenzent { get; set; }

        [ForeignKey("Rad")]
        public int RadId { get; set; }
        public virtual Rad Rad { get; set; }

        public bool? ApprovedByRecenzent { get; set; }
    }
}
