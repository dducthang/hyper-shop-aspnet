using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models
{
    public class SecondaryImage
    {
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; }
        [Required]
        public int ColorId { get; set; }
        [ValidateNever]
        public Color Color { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
