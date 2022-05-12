using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models
{
    public class Stock
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Product")]
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Size")]
        public int SizeId { get; set; }
        [Required]
        [Display(Name = "Color")]
        public int ColorId { get; set; }
        [Required]
        public int Quantity { get; set; }

    }
}
