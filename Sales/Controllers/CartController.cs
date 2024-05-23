using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sales.Data;
using Sales.Data.Models;
using Sales.Models.Cart;
using Sales.Models.Product;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace Sales.Controllers
{
    /// <summary>
    /// Контроллер корзины
    /// </summary>
    [Authorize]
    public class CartController : Controller
    {
        private ApplicationDbContext _context;
        private string cartKey = "cart";
        private CartModel _cart;

        public CartController(ApplicationDbContext context, CartModel cart)
        {
            _context = context;
            _cart = cart;
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
        }

        public IActionResult Index()
        {
            var cartString = HttpContext.Session.Get(cartKey);

            if (cartString == null)
            {
                _cart = new CartModel();
            }
            else
            {
                _cart = JsonSerializer.Deserialize<CartModel>(cartString)!;
            }
            return View(_cart.Items.Values);
        }

        [Authorize]
        public async Task<IActionResult> Add(int id, string returnUrl)
        {
            var cartString = HttpContext.Session.Get(cartKey);

            if (cartString == null)
            {
                _cart = new CartModel();
            }
            else
            {
                _cart = JsonSerializer.Deserialize<CartModel>(cartString)!;
            }

            var item = await _context.Products
                .Select(p => new ProductModel
                {
                    Id = p.Id,
                    Length = p.Length,
                    Name = p.Name,
                    Price = p.Price,
                    Thickness = p.Thickness,
                    Width = p.Width
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (item != null)
            {
                _cart.AddToCart(item);
                HttpContext.Session.SetString(cartKey, JsonSerializer.Serialize(_cart));
            }
            return Redirect(returnUrl);
        }

        public IActionResult Delete(int id)
        {
            _cart.RemoveFromCart(id);
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            _cart.ClearAll();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Checkout()
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdStr == null) return NotFound();

            if (!int.TryParse(userIdStr, out var userId))
            {
                return NotFound();
            }

            ViewBag.LoginedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var addresses = await _context.UserAddresses.Where(ua => ua.UserId == userId).ToListAsync();


            ViewBag.Addresses = new SelectList(addresses, "Address", "Address");

            return View(new CheckoutModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdStr == null) return NotFound();

            if (!int.TryParse(userIdStr, out var userId))
            {
                return NotFound();
            }

            ViewBag.LoginedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var addresses = await _context.UserAddresses.Where(ua => ua.UserId == userId).ToListAsync();


            ViewBag.Addresses = new SelectList(addresses, "Address", "Address");

            if (model.UseManualAddress)
            {
                if (string.IsNullOrEmpty(model.Address))
                {
                    ModelState.AddModelError(nameof(model.Address), "Необходимо ввести адрес доставки");
                }
            }
            else
            {
                if (model.SelectedAddress == null)
                {
                    ModelState.AddModelError(nameof(model.SelectedAddress), "Необходимо выбрать адрес доставки");
                }
            }

            if (ModelState.IsValid)
            {
                string selectedAddr = string.Empty;

                if (model.UseManualAddress)
                {
                    var address = await _context.UserAddresses.FirstOrDefaultAsync(a => a.Address == model.Address);

                    if (address == null)
                    {
                        address = new UserAddress
                        {
                            Address = model.Address!,
                            UserId = userId
                        };

                        await _context.UserAddresses.AddAsync(address);
                        await _context.SaveChangesAsync();

                        addresses = await _context.UserAddresses.Where(ua => ua.UserId == userId).ToListAsync();

                        ViewBag.Addresses = new SelectList(addresses, "Address", "Address");
                    }

                    selectedAddr = model.Address!;
                }
                else
                {
                    selectedAddr = model.SelectedAddress!;
                }


                var order = new Order
                {
                    CustomerId = userId,
                    Address = selectedAddr,
                    OrderedProducts = _cart.Items.Values.Select(p => new OrderedProduct
                    {
                        ProductId = p.Product.Id,
                        Quantity = p.Quantity
                    }).ToList()
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                _cart.ClearAll();

                return View("CheckoutSuccess");
            }

            return View(model);
        }
    }
}