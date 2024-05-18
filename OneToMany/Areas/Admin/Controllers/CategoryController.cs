using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneToMany.Data;
using OneToMany.Models;
using OneToMany.ViewModels.Categories;

namespace OneToMany.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.OrderByDescending(m=>m.Id).ToListAsync();
            List<CategoryVM> model = categories.Select(m => new CategoryVM { Id = m.Id, Name = m.Name }).ToList();
            //List<CategoryVM> model = new();
            //foreach (var item in categories)
            //{
            //    model.Add(new CategoryVM
            //    {
            //        Id = item.Id,
            //        Name = item.Name
            //    });
            //}
            return View(model);
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
            await _context.Categories.AddAsync(new Category { Name = category.Name });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if(id is null)
            {
                return BadRequest();
            }
            Category category = await _context.Categories.Where(m => m.Id == id).Include(m => m.Products).FirstOrDefaultAsync();
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
            Category category = await _context.Categories.Where(m => m.Id == id).Include(m => m.Products).FirstOrDefaultAsync();
            if (category is null) return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            Category category = await _context.Categories.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (category is null) return NotFound();
            return View(new CategoryEditVM {  Id = category.Id , Name = category.Name });
        }



        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id , CategoryEditVM category)
        {
            if (!ModelState.IsValid) return View();
            if (id is null) return BadRequest();
            Category existCategory = await _context.Categories.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (existCategory is null) return NotFound();
            existCategory.Name = category.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}


