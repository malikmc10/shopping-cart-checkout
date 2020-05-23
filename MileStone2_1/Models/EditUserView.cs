using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MileStone2_1.Models
{
    public class EditUserView
    {
        public EditUserView()
        { 
        Claims = new List<string>(); 
        Roles = new List<string>();
            
        }
        public string UserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }


    }

       



    }

