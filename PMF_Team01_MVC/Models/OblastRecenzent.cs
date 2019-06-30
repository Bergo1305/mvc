﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models
{
    public class OblastRecenzent
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        
        [ForeignKey("Oblast")]
        public int OblastId { get; set; }
        public virtual Oblast Oblast { get; set; }
    }
}
