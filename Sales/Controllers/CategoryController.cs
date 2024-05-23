using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.Data;
using Sales.Data.Models;
using Sales.Models.Category;

namespace Sales.Controllers
{
    /// <summary>
    /// Контроллер категории
    /// </summary>
    [Authorize(Roles = "Administrator, Manager")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        private readonly IWebHostEnvironment _env;

        public CategoryController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        
        public IActionResult Create()
        {
            var category = new CreateCategoryModel();
            return View(category);
        }


        /// <summary>
        ///  Создать категорию
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryModel model)
        {
            if(ModelState.IsValid)
            {
                var category = await _db.Categories.FirstOrDefaultAsync(c => c.Name.Equals(model.Name));

                if(category != null) {
                    ModelState.AddModelError(nameof(model.Name), "Наименование категории уже присутствуетв базе данных");
                    return View(model);
                }

                var imageCat = Path.Combine(_env.WebRootPath, "img", "categories");

                if (!Directory.Exists(imageCat))
                {
                    Directory.CreateDirectory(imageCat);
                }

                if (model.Image != null)
                {
                    using var fs = new FileStream(Path.Combine(imageCat, model.Image.FileName), FileMode.Create);
                    await model.Image.CopyToAsync(fs);
                }

                await _db.Categories.AddAsync(new Category
                {
                    Name = model.Name,
                    Image = model.Image != null ? Path.Combine("categories", model.Image.FileName) : null
                });

                await _db.SaveChangesAsync();

                return RedirectToAction("Index", "Catalog");
            }
            return View(model);
        }

        /// <summary>
        ///  Изменить категорию
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var category = await _db.Categories.Select(c => new EditCategoryModel
            {
                Id = c.Id,
                Name = c.Name,
            }).FirstOrDefaultAsync(c => c.Id == id);

            return View(category);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == model.Id);

                if(category == null)
                {
                    return NotFound();
                }

                var imageCat = Path.Combine(_env.WebRootPath, "img", "categories");

                if (!Directory.Exists(imageCat))
                {
                    Directory.CreateDirectory(imageCat);
                }

                if (model.Image != null)
                {
                    using var fs = new FileStream(Path.Combine(imageCat, model.Image.FileName), FileMode.Create);
                    await model.Image.CopyToAsync(fs);
                    category.Image = Path.Combine("categories", model.Image.FileName);
                }

                category.Name = model.Name;

                _db.Categories.Update(category);

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

            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category != null)
            {
                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), "Catalog");
        }
    }
}
