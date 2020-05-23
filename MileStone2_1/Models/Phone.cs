using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MileStone2_1.Models
{
    public class Phone
    {
        [Key]
      //  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Phone Id")]
        public int Phone_Id { get; set; }
        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Manufacture_Date { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
