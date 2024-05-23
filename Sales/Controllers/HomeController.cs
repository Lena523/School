using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.Data;
using Sales.Models.Category;
using System.Security.Claims;

namespace Sales.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var categories = await _context.Categories.Select(c => new CategoryModel
            {
                Id = c.Id,
                Image = c.Image,
                Name = c.Name
            }).ToListAsync();

            return View(categories);
        }

        public async Task<IActionResult> ProfileAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Name = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "Name").Value;
                ViewBag.Surname = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "Surname").Value;
                ViewBag.Gender = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "Gender").Value;
                ViewBag.Phone = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "Phone").Value;
                ViewBag.Telegram = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "Telegram").Value;
                ViewBag.BirthDate = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "BirthDate").Value;
            }


            return View();
        }
    }
}
