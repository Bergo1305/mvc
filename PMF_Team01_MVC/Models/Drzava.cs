﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models
{
    public class Drzava
    {
        [Key]
        public int Id { get; set; }

        public string Naziv { get; set; }
    }
}
