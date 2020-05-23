using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace MileStone2_1.Models
{
    public class CreateRoleView
    {
        [Required]
        public string RoleName { get; set; }
        

    }
}
