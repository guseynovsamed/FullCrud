using System;
using OneToMany.Models;
using System.ComponentModel.DataAnnotations;

namespace OneToMany.ViewModels.Products
{
	public class ProductEditVM
	{
        public int? Id { get; set; }
        [StringLength(20, ErrorMessage = "Length must be max 20")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public List<ProductImage>? Image { get; set; }
        public List<IFormFile>? NewImages { get; set; }
    }
}
