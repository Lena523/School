#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Sales.Data.Models;
using Sales.Models;

namespace Sales.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }


            [Required]
            [StringLength(100, ErrorMessage = "Длина {0} должна быть не менее {2} и не более {1} символов", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Подтвердить пароль")]
            [Compare("Password", ErrorMessage = "Пароли не совпадают")]
            public string ConfirmPassword { get; set; }

            [DataType(DataType.Date)]
            public DateTime BirthDate { get; set; } 

            public Gender Gender { get; set; }

            public string Name { get; set; }
            public string Surname { get; set; }

            public string Phone {  get; set; }
            public string Telegram { get; set; }

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Пользователь создал новую учетную запись с паролем");

                    await _userManager.AddToRoleAsync(user, "User");

                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(nameof(Input.Name), Input.Name));
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(nameof(Input.Surname), Input.Surname));
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(nameof(Input.BirthDate), Input.BirthDate.ToShortDateString()));
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(nameof(Input.Phone), Input.Phone));
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(nameof(Input.Telegram), Input.Telegram));
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(nameof(Input.Gender), Input.Gender.ToString()));

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Подтверждение email",
                        $"Пожалуйста, подтвердите email по ссылке <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Кликнуть сюда</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Если мы зашли так далеко, что-то пошло не так
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Не могу создать экземпляр '{nameof(ApplicationUser)}'. " +
                    $"Гарантировать, что '{nameof(ApplicationUser)}' не является абстрактным классом и имеет конструктор без параметров или, альтернативно, " +
                    $"переопределить страницу регистрации in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("Для пользовательского интерфейса по умолчанию требуется хранилище пользователей с поддержкой по электронной почте.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}