using System;
namespace OneToMany.ViewModels
{
	public class HeaderVM
	{
		public Dictionary<string,string> Setting { get; set; }
		public int BasketCount { get; set; }
		public decimal BasketTotalPrice { get; set; }
	}
}

