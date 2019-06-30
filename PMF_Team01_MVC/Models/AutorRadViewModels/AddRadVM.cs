using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PMF_Team01_MVC.Models.AuthorTitles
{
    //za studentske radove i recenzirane radove
    public class AddRadVM
    {
        [Required]
        [MinLength(10)]
        public string Naslov { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 300, ErrorMessage = "Abstract must be at least 300 characters long")]
        public string Apstrakt { get; set; }
        [Required]
        public string KeyWords { get; set; }
        [Required]
        public string TipRada { get; set; }
        public bool ApprovedByAdmin { get; set; }
        public DateTime? PublicationDate { get; set; }

        public string OstaliAutori { get; set; }

        public IFormFile Document { get; set; }

        //za recenzirani rad
        public string TipRecenziranogRada { get; set; }
        public List<SelectListItem> TipRecenziranogRadaList { get; set; }
        
        
        //za studentske radove
        public string Mentor { get; set; }
        public List<SelectListItem> MentoriList { get; set; }
        public string TipStudentskogRada { get; set; }
        public List<SelectListItem> TipStudentskogRadaList { get; set; }

        //Ideja
        [StringLength(int.MaxValue, MinimumLength = 300, ErrorMessage = "Idea text must be at least 300 characters long")]
        public string TekstIdeje { get; set; }

    }
}
