using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PMF_Team01_MVC.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        public string Id { get; set; }

        //zakomentarisano zbog toga sto se email ne koristi u edit account
        //a u registraciji je obavezan... rijestiti to u kontroleru
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d+).{8,}$",
         ErrorMessage = "Password must contain digits, uppercase and lowercase letters!")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d+).{8,}$",
         ErrorMessage = "Password must contain digits, uppercase and lowercase letters!")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Affiliation { get; set; }
        [Required]
        public string Initials { get; set; }

        //[Required]
        public string Title { get; set; }
        
        public string Role { get; set; }
        public List<SelectListItem> Roles { get; set; }

        public IFormFile Image { get; set; }

        public IFormFile CV { get; set; }
        
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }
}
