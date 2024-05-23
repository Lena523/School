using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.Data;
using Sales.Data.Models;

namespace Sales.Controllers
{
    /// <summary>
    /// Контроллер заказов
    /// </summary>
    public class OrdersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Контруктор класса
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="userManager">Сервис управления учетными записями пользователей</param>
        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Метод получения списка заказов
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            IQueryable<Order> applicationDbContext;
            var user = await _userManager.GetUserAsync(User);
            if (User.IsInRole("User") && user != null)
            {
                applicationDbContext = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.OrderedProducts)
                .ThenInclude(op => op.Product)
                .Where(o => o.CustomerId == user.Id)
                .AsQueryable();
            }
            else
            {
                applicationDbContext = _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Employee)
                    .Include(o => o.OrderedProducts)
                    .ThenInclude(op => op.Product).AsQueryable();
            }

            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// Метод получения детальной информации по товару
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.OrderedProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        /// <summary>
        /// Принять в работу заказ
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <param name="returnUrl">Ссылка возврата на страницу откуда был инициирован запрос</param>
        /// <returns></returns>
        public async Task<IActionResult> Accept(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            order.EmployeeId = user.Id;
            order.AcceptedDate = DateTime.Now;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return Redirect(returnUrl);
        }

        /// <summary>
        /// Закрыть заказ
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <param name="returnUrl">Ссылка возврата на страницу откуда был инициирован запрос</param>
        /// <returns></returns>
        public async Task<IActionResult> Close(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            order.IsClosed = true;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return Redirect(returnUrl);
        }

        /// <summary>
        /// Открыть заказ
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <param name="returnUrl">Ссылка возврата на страницу откуда был инициирован запрос</param>
        /// <returns></returns>
        public async Task<IActionResult> Open(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            order.IsClosed = false;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return Redirect(returnUrl);
        }

        /// <summary>
        /// Вывод информации по заказу для формы
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        /// <summary>
        /// Отправка данных формы на удаление заказа
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Проверка наличия заказа
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <returns></returns>
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}