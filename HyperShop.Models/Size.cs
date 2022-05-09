using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models
{
    public class Size
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Size")]
        public double SizeValue { get; set; }
    }
}
