using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models
{
    public class KomentarDokument
    {
        [ForeignKey("PrivatniKomentar")]
        public int KomentarDokumentId { get; set; }

        public string FileName { get; set; }

        public virtual PrivatniKomentar PrivatniKomentar { get; set; }
    }
}
