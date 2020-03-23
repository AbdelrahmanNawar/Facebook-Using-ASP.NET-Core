using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress]
        public string LoginEmail { get; set; }
        
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }


    }
}
