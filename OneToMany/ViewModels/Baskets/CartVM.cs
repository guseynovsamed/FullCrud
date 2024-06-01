using System;
namespace OneToMany.ViewModels.Baskets
{
	public class CartVM
	{
		public List<BasketProductsVM>? BasketProducts { get; set; }
		public decimal Total { get; set; }
			
	}
}

