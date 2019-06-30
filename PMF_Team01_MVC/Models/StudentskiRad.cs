using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models
{
    public class StudentskiRad
    {
        [ForeignKey("Rad")]
        public int StudentskiRadId { get; set; }
        public virtual Rad Rad { get; set; }

        [ForeignKey("Mentor")]
        public int MentorId { get; set; }
        public virtual Mentor Mentor { get; set; }

        public string TipStudentskogRada { get; set; } //seminarski, zavrsni i sl.
        public string Napomena { get; set; }
    }
}
