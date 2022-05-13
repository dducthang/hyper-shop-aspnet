using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class StockVM
    {
        public int ProductId { get; set; }
        public List<Color> Stock { get; set; }  
    }

}
