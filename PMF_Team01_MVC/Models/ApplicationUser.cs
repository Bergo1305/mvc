using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PMF_Team01_MVC.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Middlename { get; set; }
        public string Initials { get; set; }
        public string Title { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Affiliation { get; set; }
        public string StudentStudyProgram { get; set; }
        public bool? IsEnabled { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        //[ForeignKey("Grad")]
        //public int GradId { get; set; }

        //public virtual Grad Grad { get; set; }
    }
}
