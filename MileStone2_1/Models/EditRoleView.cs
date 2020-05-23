using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MileStone2_1.Models
{
    public class EditRoleView
    {

        public EditRoleView()
        {
            Users = new List<string>();
        }
        public string Id { get; set; }

        [Required (ErrorMessage ="Role Name Required")]
          public string RoleName { get; set; }

        public List<string>  Users { get; set; }
    }
}
