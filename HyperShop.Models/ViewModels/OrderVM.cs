using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class OrderVM
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string District { get; set; }
        public string City { get; set; }

        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public double Total { get; set; }
        public double ShipCost { get; set; }
        public List<OrderVmDetail> Items { get; set; }
    }

    public class OrderVmDetail
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Size { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public double PricePerUnit { get; set; }

    }
}
