using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models
{
    public class KategorijaIdeja
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Kategorija")]
        public int KategorijaId { get; set; }
        public virtual Kategorija Kategorija { get; set; }

        [ForeignKey("Ideja")]
        public int IdejaId { get; set; }
        public virtual Ideja Ideja { get; set; }
    }
}
