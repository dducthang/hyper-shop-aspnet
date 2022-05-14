using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public string Gender { get; set; }
        [Display(Name ="Shoes Height")]
        public string ShoesHeight { get; set; }
        [Display(Name = "Closure Type")]
        public string ClosureType { get; set; }
        [Display(Name = "Published Date")]
        public DateTime PublishedDate { get; set; } = DateTime.Now;
        [Display(Name ="Image")]
        public string PrimaryImage { get; set; }
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        [Display(Name = "Brand")]
        public int? BrandId { get; set; }
        public Brand Brand { get; set; }

    }
}
