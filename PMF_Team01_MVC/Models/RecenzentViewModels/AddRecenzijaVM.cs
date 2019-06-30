using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PMF_Team01_MVC.Models.RecenzentViewModels
{
    public class AddRecenzijaVM
    {
        [Required]
        public int EKnjigaId { get; set; }
        [Required]
        public string RecenzentId { get; set; }

        public string Naziv { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public IFormFile File { get; set; }
    }
}
