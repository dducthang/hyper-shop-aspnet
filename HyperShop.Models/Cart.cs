using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
        [ValidateNever]
        public ApplicationUser User { get; set; }  
        public int StockId { get; set; }
        [ValidateNever]
        public Stock Stock { get; set; }
        public double Price { get; set; }



    }
}
