using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.ViewModels
{
    public class AboutViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Bio { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime Birthdate { get; set; }

        public int GetGender()
        {
            switch (Gender)
            {
                case "Female":
                    return 0;
                case "Male":
                    return 1;
                default:
                    return 2;
            }
        }
    }
}
