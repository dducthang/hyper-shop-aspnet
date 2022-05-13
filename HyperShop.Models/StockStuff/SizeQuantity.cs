using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.StockStuff
{
    public class SizeQuantity
    {
        public int StockId { get; set; }
        public int SizeId { get; set; }
        public double Size { get; set; }
        public int Qty { get; set; }
    }
}
