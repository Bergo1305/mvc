using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }
        public string Version { get; set; }

        [ForeignKey("Rad")]
        public int RadId { get; set; }

        public virtual Rad Rad { get; set; }

        public bool TrenutnaVerzija { get; set; }
    }
}
