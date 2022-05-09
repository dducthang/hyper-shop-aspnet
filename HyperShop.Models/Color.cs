using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models
{
    public class Color
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Color")]
        public string ColorValue { get; set; }
    }
}
