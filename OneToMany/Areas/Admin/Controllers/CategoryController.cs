using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneToMany.Data;
using OneToMany.Models;
using OneToMany.Services.Interface;
using OneToMany.ViewModels.Categories;

namespace OneToMany.Areas.Admin.Controllers
{   
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;

        public CategoryController(AppDbContext context,
                                  ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService; 
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
    
            //List<CategoryVM> model = new();
            //foreach (var item in categories)
            //{
            //    model.Add(new CategoryVM
            //    {
            //        Id = item.Id,
            //        Name = item.Name
            //    });
            //}
            return View(await _categoryService.GetAllOrderByDescAsync());
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM category)
        {
            if (!ModelState.IsValid) return View();
            bool existCategory = await _categoryService.ExistAsync(category.Name);
            if (existCategory) { ModelState.AddModelError("Name", "This category already exist"); return View();}
            await _categoryService.CreateAsync(category);
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if(id is null)
            {
                return BadRequest();
            }
            Category category = await _categoryService.GetWithProductAsync((int)id);
            if (category is null) return NotFound();
            CategoryDetailVM model = new CategoryDetailVM()
            {
                Name = category.Name,
                ProductCount = category.Products.Count()
            };
            return View(model);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            Category category = await _categoryService.GetWithProductAsync((int)id);
            if (category is null) return NotFound();
            _categoryService.DeleteAsync(category);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            Category category = await _categoryService.GetByIdAsync((int)id);
            if (category is null) return NotFound();
            return View(new CategoryEditVM {  Id = category.Id , Name = category.Name });
        }



        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id , CategoryEditVM category)
        {
            if (!ModelState.IsValid) return View();
            if (id is null) return BadRequest();
            Category existCategory = await _categoryService.GetByIdAsync((int)id);
            if (existCategory is null) return NotFound();
            await _categoryService.EditAsync(existCategory, category);
            return RedirectToAction(nameof(Index));
        }

    }
}


