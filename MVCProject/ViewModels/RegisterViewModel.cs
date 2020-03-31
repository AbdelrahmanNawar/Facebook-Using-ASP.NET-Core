using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.ViewModels
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage ="Atleast 5 digits")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Does not match the Password.")]
        public string ConfirmedPassword { get; set; }

        public DateTime Birthday { get; set; }
        public string Gender { get; set; }

        public string RoleName { get; set; }
    }
}
