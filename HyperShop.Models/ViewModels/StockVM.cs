using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class StockVM
    {
        public int ProductId { get; set; }
        public List<ColorStock> Stock { get; set; }  
    }
    public class ColorStock
    {
        public int Id { get; set; }
        [Display(Name ="Color")]
        public string ColorValue { get; set; }
        public string? Image { get; set; }
    }
}
