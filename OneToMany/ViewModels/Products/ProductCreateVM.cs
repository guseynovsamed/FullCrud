using System;
using System.ComponentModel.DataAnnotations;

namespace OneToMany.ViewModels.Products
{
	public class ProductCreateVM
	{
		[Required]
		public string? Name { get; set; }
		[Required]
		public string? Description { get; set; }
        [Required(ErrorMessage = "Price can't be empty")]
        public string? Price { get; set; }
		public int CategoryId { get; set; }
        [Required]
        public List<IFormFile>? Image { get; set; }
	}
}

