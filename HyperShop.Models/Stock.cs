using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        public Product Product { get; set; }
        [Required]
        [Display(Name = "Size")]
        public int SizeId { get; set; }
        [ValidateNever]
        public Size Size { get; set; }
        [Required]
        [Display(Name = "Color")]
        public int ColorId { get; set; }
        [ValidateNever]
        public Color Color { get; set; }
        [Required]
        public int Quantity { get; set; }

    }
}
