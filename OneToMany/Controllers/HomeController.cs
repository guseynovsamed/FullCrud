
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneToMany.Data;
using OneToMany.Models;
using OneToMany.Services.Interface;
using OneToMany.ViewModels;

namespace OneToMany.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _context;
        private readonly IProductService _productService;
		private readonly ICategoryService _categoryService;
		private readonly ISayService _sayService;
		private readonly ISliderInstaSerivce _sliderInstaSerivce;


        public HomeController(AppDbContext context,
							  IProductService productService,
							  ICategoryService categoryService,
							  ISayService sayService,
							  ISliderInstaSerivce sliderInstaSerivce)
		{
			_context = context;
            _productService = productService;
			_categoryService = categoryService;
			_sayService = sayService;
			_sliderInstaSerivce = sliderInstaSerivce;
        }

        public async Task<IActionResult> Index()
		{
			List<Slider> sliders = await _context.Sliders.ToListAsync();
			SliderInfo? sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync();
			List<Category> categories = await _categoryService.GetAllAsync();
			List<Product> products = await _productService.GetAllAsync();
			List<Blog> blogs = await _context.Blogs.Take(3).ToListAsync();
			List<Say> says = await _sayService.GetAllAsync();
			List<SliderInsta> sliderInstas = await _sliderInstaSerivce.GetAllAsync();

			HomeVM model = new()
			{
				Sliders = sliders,
				SliderInfo = sliderInfo,
				Categories=categories,
				Products=products,
				Blogs=blogs,
				Says = says,
				SliderInstas=sliderInstas
			};

			return View(model);
		}
	}
}

