using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sales.Data.Models;
using Sales.Models.Employee;

namespace Sales.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class EmployeesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;

        public EmployeesController(UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync("Manager");

            return View(users.Select(u => new EmployeeModel
            {
                Id = u.Id,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Name = u.Name
            }));
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _userManager.FindByIdAsync(id.ToString()!);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeModel employeeModel)
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser
                {
                    Name = employeeModel.Name,
                    Email = employeeModel.Email,
                    PhoneNumber = employeeModel.PhoneNumber,
                };

                await _userStore.SetUserNameAsync(newUser, employeeModel.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(newUser, employeeModel.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(newUser, employeeModel.Password);

                if (result.Succeeded)
                {

                    var user = await _userManager.FindByEmailAsync(employeeModel.Email);

                    if(user != null)
                    {
                        if(!await _userManager.IsInRoleAsync(user, "Manager"))
                        {
                            await _userManager.AddToRoleAsync(user, "Manager");
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(employeeModel);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id.ToString()!);

            if (user == null)
            {
                return NotFound();
            }

            var empl = new EditEmployeeModel
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Name = user.Name
            };

            return View(empl);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditEmployeeModel employeeModel)
        {
            if (id != employeeModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(employeeModel.Id.ToString()!);

                if (user != null)
                {

                    user.Name = employeeModel.Name;
                    user.Email = employeeModel.Email;
                    user.PhoneNumber = employeeModel.PhoneNumber;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                       
                        return RedirectToAction(nameof(Index));
                        
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(employeeModel);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id.ToString()!);

            if (user == null)
            {
                return NotFound();
            }

            var empl = new EmployeeModel
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Name = user.Name
            };

            return View(empl);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString()!);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ApplicationUserExistsAsync(int id)
        {
            return (await _userManager.FindByIdAsync(id.ToString()!)) != null;
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
