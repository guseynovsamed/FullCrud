using System;
using OneToMany.Models;

namespace OneToMany.Services.Interface
{
	public interface ISliderInstaSerivce
	{
		Task<List<SliderInsta>> GetAllAsync();
	}
}

