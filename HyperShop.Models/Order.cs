using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models
{
    public class Order
    {
        [ValidateNever]
        public int Id { get; set; }
        [Required]
        [ValidateNever]
        public string UserId { get; set; }
        [ValidateNever]
        public ApplicationUser User { get; set; }
        public string Name { get; set; } 
        public string Phone { get; set; }
        [Display(Name="Street Address")]
        public string StreetAddress { get; set; }
        public string District { get; set; }
        [Display(Name = "City")]
        public string CityName { get; set; }
        [ValidateNever]
        public string Status { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [ValidateNever]
        public double TotalPrice { get; set; }
        [ValidateNever]
        public double ShipCost { get; set; }
        [ValidateNever]
        public List<OrderDetail> OrderDetails { get; set; }



    }
}
