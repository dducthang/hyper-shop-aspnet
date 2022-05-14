using HyperShop.Models;
using HyperShop.Models.StockStuff;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class StockUpsertVM
    {   
        [Required]
        public int ProductId { get; set; }
        [Display(Name ="Color")]
        [Required]
        public int ColorId { get; set; }
        [Display(Name = "Primary Image")]
        public string PrimaryImage { get; set; }
        [Display(Name = "Secondary Images")]
        public List<string> SecondaryImages { get; set; }
        [Required]
        public List<SizeQuantity> SizeQty { get; set; }

    }
}
