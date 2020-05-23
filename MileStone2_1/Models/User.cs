using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MileStone2_1.Models
{
    public class User
    {
        
        public string UserId { get; set; }
       
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        [Compare("Password", ErrorMessage ="Password Do Not Match.")]  
        public string ConfirmPassword { get; set; }


    }
}
