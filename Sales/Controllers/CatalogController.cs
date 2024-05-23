using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.Data;
using Sales.Data.Models;
using Sales.Models.Catalog;
using System.Globalization;

namespace Sales.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CatalogController(ApplicationDbContext db)
        {
            _db = db;
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
        }

        public async Task<IActionResult> IndexAsync(int? id)
        {
            var categoryMenu = new List<MenuItemModel>
            {
                new() {
                    Name = "Все"
                }
            };

            var categories = await _db.Categories.Select(c => new MenuItemModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();

            categoryMenu.AddRange(categories);


            categoryMenu.ForEach(item =>
            {
                if (id == null)
                {
                    if(item.Name == "Все")
                    {
                        item.Active = "active";
                    }
                }
                else
                {
                    if (item.Id.Equals(id))
                    {
                        item.Active = "active";
                    }
                }
            });

            ViewData["Categories"] = categoryMenu;

            var products = new List<Product>();

            if (id != null)
            {
                products = await _db.Products.Include(p => p.Category)
                    .Where(p => p.Category.Id.Equals(id))
                    .ToListAsync();
            }
            else
            {
                products = await _db.Products.Include(p => p.Category).ToListAsync();
            }

            return View(products);
        }
    }
}
