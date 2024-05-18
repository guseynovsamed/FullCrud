using System;
using OneToMany.Models;

namespace OneToMany.Services.Interface
{
	public interface ICategoryService
	{
		Task<List<Category>> GetAllAsync();
	}
}

