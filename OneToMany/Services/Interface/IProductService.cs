using System;
using System.Threading.Tasks;
using OneToMany.Models;
using OneToMany.ViewModels.Categories;
using OneToMany.ViewModels.Products;

namespace OneToMany.Services.Interface
{
	public interface IProductService
	{
		Task<List<Product>> GetAllWithImageAsync();
		Task<Product> GetByIdAsync(int? id);
        Task<List<Product>> GetAllAsync();
		List<ProductVM> GetMappedDatas(List<Product> products);
		Task<List<Product>> GetAllPaginateAsync(int page , int take = 4);
		Task<int> GetCountAsync();
		Task CreateAsync(Product product);
		Task DeleteAsync(Product product);
    }
}

