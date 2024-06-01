using System;
namespace OneToMany.Models
{
	public class Setting : BaseEntity
	{
		public string? Key { get; set; }
		public string? Value { get; set; }
	}
}

