using System;
using OneToMany.Models;

namespace OneToMany.Services.Interface
{
	public interface IProductService
	{
		Task<List<Product>> GetAllAsync();
		Task<Product> GetByIdAsync(int? id);
	}
}

