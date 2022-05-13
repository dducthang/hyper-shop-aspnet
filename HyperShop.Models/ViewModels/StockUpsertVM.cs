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
        public int ProductId { get; set; }
        [Display(Name ="Color")]
        public int ColorId { get; set; }
        public string PrimaryImage { get; set; }
        public List<string> SecondaryImages { get; set; }
        public List<SizeQuantity> SizeQty { get; set; }

    }
}
