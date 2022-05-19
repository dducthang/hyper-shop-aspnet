using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class ProductsPageVM
    {
        public List<Brand> Brands { get; set; }
        public List<Category> Categories { get; set; }
        public List<Color> Colors { get; set; }
        public List<string> Genders { get; set; }
        public List<string> ShoesHeightTypes { get; set; }
        public List<string> ClosureTypes { get; set; }
    }
}
