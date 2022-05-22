using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class NavVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Brand> Brands{ get; set; }
    }
}
