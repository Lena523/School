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
    public class EditAddressModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;

        public EditAddressModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

        [BindProperty]
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Id = id;
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Невозможно загрузить пользователя с ID '{_userManager.GetUserId(User)}'.");
            }

            var address = await _context.UserAddresses.FirstOrDefaultAsync(ua => ua.Id == id);

            if (address == null)
            {
                return NotFound($"Невозможно загрузить адрес с ID '{id}'.");
            }

            Address = address.Address;


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, string address)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound($"Невозможно загрузить пользователя с ID '{_userManager.GetUserId(User)}'.");
                }

                var addressEn = await _context.UserAddresses.FirstOrDefaultAsync(ua => ua.Id == id);

                addressEn.Address = address;

                _context.UserAddresses.Update(addressEn);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Address");
            }

            return Page();
        }
    }
}