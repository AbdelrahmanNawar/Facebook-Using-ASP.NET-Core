using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.ViewModels
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Atleast 5 digits")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Atleast 5 digits")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Does not match the Password.")]
        public string ConfirmedPassword { get; set; }

    }
}
