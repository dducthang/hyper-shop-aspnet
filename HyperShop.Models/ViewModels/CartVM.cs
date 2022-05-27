using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class CartVM
    {
        public int CartId { get; set; }
        public string ProductName { get; set; }
        public double Size { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
        public int StockQuantity { get; set; }
        public double Price { get; set; }

        //for anonymous cart
        public int StockId { get; set; }



    }
}
