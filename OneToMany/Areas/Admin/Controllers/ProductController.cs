using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneToMany.Data;
using OneToMany.Helpers;
using OneToMany.Helpers.Extension;
using OneToMany.Models;
using OneToMany.Services.Interface;
using OneToMany.ViewModels.Categories;
using OneToMany.ViewModels.Products;

namespace OneToMany.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;

        public ProductController(IProductService productService,
                                 ICategoryService categoryService,
                                 IWebHostEnvironment env,
                                 AppDbContext context)
        {
            _productService = productService;
            _categoryService = categoryService;
            _env = env;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var paginateDatas = await _productService.GetAllPaginateAsync(page);



            var mappeDatas = _productService.GetMappedDatas(paginateDatas);

            int pageCount = await GetPageCountAsync(4);

            Paginate<ProductVM> model = new(mappeDatas, pageCount, page);

            return View(model);
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var count = await _productService.GetCountAsync();

            return (int)Math.Ceiling((decimal)count / take);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Product product = await _productService.GetByIdAsync((int)id);

            if (product is null) return NotFound();

            List<ProductImageVM> productImages = new();

            foreach (var item in product.ProductImages)
            {
                productImages.Add(new ProductImageVM
                {
                    Image = item.Name,
                    IsMain = item.IsMain
                });
            }

            ProductDetailVM model = new()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category.Name,
                Image = productImages
            };

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await _categoryService.GetAllBySelectedAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            ViewBag.categories = await _categoryService.GetAllBySelectedAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            foreach (var item in request.Image)
            {
                if (!item.CheckFileSize(500))
                {
                    ModelState.AddModelError("Image", "Image size must be max 500kb");
                    return View();
                }

                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Image", "File size must be only Image");
                    return View();
                }
            }

            List<ProductImage> images = new();


            foreach (var item  in request.Image)
            {
                string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;

                string path = Path.Combine(_env.WebRootPath, "img", fileName);

                await item.SaveFileToLocalAsync(path);


                images.Add(new ProductImage
                {
                    Name = fileName
                });
            }

            images.FirstOrDefault().IsMain = true;


            Product product = new()
            {
                Name = request.Name,
                Description = request.Description,
                Price = decimal.Parse(request.Price.Replace(".",",")),
                CategoryId = request.CategoryId,
                ProductImages = images
            };

            await _productService.CreateAsync(product);



            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            Product product = await _productService.GetByIdAsync((int)id);

            if (product is null) return NotFound();

            foreach (var item in product.ProductImages)
            {
                string path = Path.Combine(_env.WebRootPath, "img", item.Name);

                path.DeleteFileFromLocal();
            }

             await _productService.DeleteAsync(product);

            return RedirectToAction(nameof(Index));
                
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.categories = await _categoryService.GetAllBySelectedAsync();

            if (id is null) return BadRequest();
            Product product = await _productService.GetByIdAsync((int)id);
            if (product is null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            List<ProductImage> images = new();

            foreach (var item in product.ProductImages)
            {

                images.Add(new ProductImage
                {
                    Name = item.Name,
                    IsMain = item.IsMain
                });
            }

            return View( new ProductEditVM
            {
                Id=product.Id,
                Name=product.Name,
                Price=product.Price,
                Description=product.Description,
                CategoryId=product.CategoryId,
                Image=images
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ProductEditVM productEdit)
        {
            ViewBag.categories = await _categoryService.GetAllBySelectedAsync();
            if (!ModelState.IsValid) return View();
            if (id is null) return BadRequest();
            Product existProduct = await _productService.GetByIdAsync((int)id);
            if (existProduct is null) return NotFound();

            List<ProductImage> images = existProduct.ProductImages.ToList();

            if (productEdit.NewImages != null)
            {
                foreach (var item in productEdit.NewImages)
                {
                    if (!item.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("NewImages", "File must be only image format");
                        productEdit.Image = images;
                        return View(productEdit);
                    }

                    if (!item.CheckFileSize(500))
                    {
                        ModelState.AddModelError("NewImages", "Image size must be max 500 kb");
                        productEdit.Image = images;
                        return View(productEdit);
                    }
                }


                foreach (var item in productEdit.NewImages)
                {
                    string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;

                    string newPath = Path.Combine(_env.WebRootPath, "img", fileName);

                    await item.SaveFileToLocalAsync(newPath);


                    ProductImage image = new()
                    {
                        Name = fileName
                    };

                    images.Add(image);

                }
            }

            existProduct.ProductImages = images;
            existProduct.Name = productEdit.Name;
            existProduct.Description = productEdit.Description;
            existProduct.Price = productEdit.Price;
            existProduct.CategoryId = productEdit.CategoryId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}