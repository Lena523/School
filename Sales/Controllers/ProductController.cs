using Microsoft.AspNetCore.Mvc;
using Sales.Data.Models;
using Sales.Data;
using Sales.Models.Category;
using Microsoft.EntityFrameworkCore;
using Sales.Models.Product;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace Sales.Controllers
{
    [Authorize(Roles = "Administrator, Manager")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryId = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");
            var product = new CreateProductModel();
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductModel model)
        {
            ViewBag.CategoryId = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");

            if (ModelState.IsValid)
            {
                var imageCat = Path.Combine(_env.WebRootPath, "img", "products");

                if (!Directory.Exists(imageCat))
                {
                    Directory.CreateDirectory(imageCat);
                }

                if (model.Image != null)
                {
                    using var fs = new FileStream(Path.Combine(imageCat, model.Image.FileName), FileMode.Create);
                    await model.Image.CopyToAsync(fs);
                }

                await _db.Products.AddAsync(new Product
                {
                    Name = model.Name,
                    CategoryId = model.CategoryId,
                    Length = model.Length,
                    Price = model.Price,
                    Thickness = model.Thickness,
                    Width = model.Width,
                    Image = model.Image != null ? Path.Combine("products", model.Image.FileName) : null
                });

                await _db.SaveChangesAsync();

                return RedirectToAction("Index", "Catalog");
            }
            return View(model);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.CategoryId = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");

            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.Products.Select(c => new EditProductModel
            {
                Id = c.Id,
                Name = c.Name,
                CategoryId = c.CategoryId,
                Length = c.Length,
                Width = c.Width,
                Price = c.Price,
                Thickness = c.Thickness
            }).FirstOrDefaultAsync(c => c.Id == id);

            product.Thickness = Math.Round(product.Thickness, 2);
            product.Length = Math.Round(product.Length, 2);
            product.Width = Math.Round(product.Width, 2);
            product.Price = Math.Round(product.Price, 2);

            return View(product);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductModel model)
        {
            ViewBag.CategoryId = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");

            if (ModelState.IsValid)
            {
                var product = await _db.Products.FirstOrDefaultAsync(c => c.Id == model.Id);

                if (product == null)
                {
                    return NotFound();
                }

                var imageCat = Path.Combine(_env.WebRootPath, "img", "products");

                if (!Directory.Exists(imageCat))
                {
                    Directory.CreateDirectory(imageCat);
                }

                if (model.Image != null)
                {
                    using var fs = new FileStream(Path.Combine(imageCat, model.Image.FileName), FileMode.Create);
                    await model.Image.CopyToAsync(fs);
                    product.Image = Path.Combine("products", model.Image.FileName);
                }

                product.Name = model.Name;
                product.CategoryId = model.CategoryId;
                product.Length = model.Length;
                product.Width = model.Width;
                product.Thickness = model.Thickness;
                product.Price = model.Price;

                _db.Products.Update(product);

                await _db.SaveChangesAsync();

                return RedirectToAction("Index", "Catalog");
            }
            return View(model);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.Products
                .Include(p => p.Category)
                .Select(p => new ProductModel
                {
                    Id = p.Id,
                    CategoryId = p.CategoryId,
                    Name = p.Name,
                    Image = p.Image,
                    Length = p.Length,
                    Width = p.Width,
                    Price = p.Price,
                    Thickness = p.Thickness,
                    Category = new CategoryModel
                    {
                        Id = p.CategoryId,
                        Name = p.Category.Name
                    }
                })
            .FirstOrDefaultAsync(c => c.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            product.Thickness = Math.Round(product.Thickness, 2);
            product.Length = Math.Round(product.Length, 2);
            product.Width = Math.Round(product.Width, 2);
            product.Price = Math.Round(product.Price, 2);

            return View(product);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), "Catalog");
        }
    }
}
