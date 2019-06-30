using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models
{
    public class Recenzija
    {
        [Key]
        public int Id { get; set; }
        
        public string FileName { get; set; }
        public string Version { get; set; }

        [ForeignKey("EKnjiga")]
        public int EKnjigaId { get; set; } //eknjiga na koju se odnosi
        public virtual EKnjiga EKnjiga { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ReviewerId { get; set; }
        public virtual ApplicationUser Reviewer { get; set; }


    }
}
