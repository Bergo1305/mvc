using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.AccountViewModels
{
    public class UpdatePasswordVM
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Current password is required!")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d+).{8,}$",
         ErrorMessage = "Password must contain digits, uppercase and lowercase letters!")]
        [Required(ErrorMessage = "New password is required!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d+).{8,}$",
         ErrorMessage = "Password must contain digits, uppercase and lowercase letters!")]
        [Required(ErrorMessage = "New password confirm is required!")]
        [Compare("NewPassword", ErrorMessage = "Confirmed password does not match!")]
        [DataType(DataType.Password)]
        public string NewPasswordConfirm { get; set; }
    }
}
