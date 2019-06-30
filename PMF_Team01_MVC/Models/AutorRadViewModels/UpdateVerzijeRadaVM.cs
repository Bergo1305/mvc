using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PMF_Team01_MVC.Models.AutorRadViewModels
{
    public class UpdateVerzijeRadaVM
    {
        public int RadId { get; set; }
        public string NazivRada { get; set; }
        [Required]
        [DataType(DataType.Upload)] //moze li stvarati probleme?
        public IFormFile Document { get; set; }
    }
}
