using System;
namespace OneToMany.Models
{
	public class Say : BaseEntity
	{
		public string? Image { get; set; }
        public string? Text { get; set; }
        public string? Author { get; set; }
	}
}

