using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HyperShop.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ValidateNever]
        public Order Order { get; set; }
        public int StockId { get; set; }
        [ValidateNever]
        public Stock Stock { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }


    }
}