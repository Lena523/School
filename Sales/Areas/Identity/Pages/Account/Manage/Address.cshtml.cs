#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sales.Data;
using Sales.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Sales.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class ListAddressModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;

        public ListAddressModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Адрес обязателен для заполнения")]
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        public List<UserAddress> Addresses { get; set; } = new List<UserAddress>();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Невозможно загрузить пользователя с ID '{_userManager.GetUserId(User)}'.");
            }

            Addresses = await _context.UserAddresses.Where(ua => ua.UserId == user.Id).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Невозможно загрузить пользователя с ID '{_userManager.GetUserId(User)}'.");
            }

            var address = await _context.UserAddresses.FirstOrDefaultAsync(ua => ua.Id == id);
            
            _context.UserAddresses.Remove(address);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound($"Невозможно загрузить пользователя с ID '{_userManager.GetUserId(User)}'.");
                }

                await _context.UserAddresses.AddAsync(new UserAddress
                {
                    UserId = user.Id,
                    Address = Address
                });
                await _context.SaveChangesAsync();

                return RedirectToPage();
            }

            return Page();
        }
    }
}