
using System;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OneToMany.Data;
using OneToMany.Models;
using OneToMany.Services.Interface;
using OneToMany.ViewModels;
using OneToMany.ViewModels.Baskets;

namespace OneToMany.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _context;
        private readonly IProductService _productService;
		private readonly ICategoryService _categoryService;
		private readonly ISayService _sayService;
		private readonly ISliderInstaSerivce _sliderInstaSerivce;
		private readonly IHttpContextAccessor _accessor;


        public HomeController(AppDbContext context,
							  IProductService productService,
							  ICategoryService categoryService,
							  ISayService sayService,
							  ISliderInstaSerivce sliderInstaSerivce,
							  IHttpContextAccessor accessor)
		{
			_context = context;
            _productService = productService;
			_categoryService = categoryService;
			_sayService = sayService;
			_sliderInstaSerivce = sliderInstaSerivce;
			_accessor = accessor;
        }

        public async Task<IActionResult> Index()
		{
			List<Category> categories = await _categoryService.GetAllAsync();
			List<Product> products = await _productService.GetAllWithImageAsync();
			List<Blog> blogs = await _context.Blogs.Take(3).ToListAsync();
			List<Say> says = await _sayService.GetAllAsync();
			List<SliderInsta> sliderInstas = await _sliderInstaSerivce.GetAllAsync();

			//string name = "Semed";

			//_accessor.HttpContext.Response.Cookies.Append("name", name);

			HomeVM model = new()
			{
				
				Categories=categories,
				Products=products,
				Blogs=blogs,
				Says = says,
				SliderInstas=sliderInstas
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> AddProductToBasket(int? id)
		{
            //_accessor.HttpContext.Request.Cookies["name"];
            //_accessor.HttpContext.Response.Cookies.Append("name", name);


            if (id is null) return BadRequest();
			List<BasketVM> basketProducts = null;
			if (_accessor.HttpContext.Request.Cookies["basket"] is not null)
			{
				basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
			}
			else
			{
				basketProducts = new List<BasketVM>();
			}

			var existProduct = basketProducts.FirstOrDefault(m => m.Id == (int)id);

			var dbProduct = await _context.Products.FirstOrDefaultAsync(m => m.Id == id); 


			if (existProduct is not null)
			{
				existProduct.Count++;
			}
			else
			{
				basketProducts.Add(new BasketVM
				{
					Id = (int)id,
					Count = 1,
					Price = (decimal)dbProduct.Price
				}) ;
			}


			_accessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketProducts));

			int count = basketProducts.Sum(m => m.Count);

			decimal total = basketProducts.Sum(m => m.Count * m.Price);

            return Ok(new { count,total });
		}
	}
}

