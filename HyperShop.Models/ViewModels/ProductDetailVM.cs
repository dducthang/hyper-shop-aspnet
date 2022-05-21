using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class ProductDetailVM
    {
        public Product Product { get; set; }
        public List<ProductColor> Colors { get; set; }
        public List<Size> Sizes { get; set; }
    }
    public class ProductColor
    {
        public int ColorId { get; set; }
        public string PrimaryImage { get; set; }
    }
}
