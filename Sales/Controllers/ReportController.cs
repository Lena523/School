using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.Data;
using Sales.Models.Report;

namespace Sales.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = "Administrator, Manager")]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> IndexAsync()
        {
            var ordersQuery = _context.Orders
                .Include(op => op.OrderedProducts)
                .ThenInclude(op => op.Product)
                .Where(o => o.CreatedDate <= DateTime.Now && o.IsClosed == true);


            var orders = await ordersQuery.Select(op => new ReportModel
                {
                    Count = op.OrderedProducts.Sum(opp => opp.Quantity),
                    Summ = op.OrderedProducts.Sum(opp => opp.Quantity * opp.Product.Price)
                })
                .ToListAsync();


            var reportProducts = await ordersQuery.SelectMany(o => o.OrderedProducts).GroupBy(op => op.Product.Name)
            .Select(g => new ReportProductModel
            {
                Name = g.Key,
                Summ = g.Sum(dd => dd.Quantity * dd.Product.Price),
                Quantity = g.Sum(dd => dd.Quantity)
            })
            .ToListAsync();

            var report = new ReportModel
            {
                ReportProducts = reportProducts,
                StartDate = null,
                EndDate = DateTime.Now,
                Count = orders.Sum(o => o.Count),
                Summ = orders.Sum(o => o.Summ)
            };

            return View(report);
        }

        /// <summary>
        /// Поиск по дате
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> IndexAsync(ReportModel model)
        {

            var ordersQuery = _context.Orders
                .Include(op => op.OrderedProducts)
                .ThenInclude(op => op.Product)
                .Where(o => o.CreatedDate <= model.EndDate && o.IsClosed == true);

            if (model.StartDate.HasValue)
            {
                ordersQuery.Where(o => o.CreatedDate >= model.StartDate);
            }

            var orders = await ordersQuery.Select(op => new ReportModel
            {
                Count = op.OrderedProducts.Sum(opp => opp.Quantity),
                Summ = op.OrderedProducts.Sum(opp => opp.Quantity * opp.Product.Price)
            })
            .ToListAsync();

            var reportProducts = await ordersQuery.SelectMany(o => o.OrderedProducts).GroupBy(op => op.Product.Name)
                .Select(g => new ReportProductModel
                {
                    Name = g.Key,
                    Summ = g.Sum(dd => dd.Quantity * dd.Product.Price),
                    Quantity = g.Sum(dd => dd.Quantity)
                })
                .ToListAsync();

            model.ReportProducts = reportProducts;
            model.Count = orders.Sum(o => o.Count);
            model.Summ = orders.Sum(o => o.Summ);

            return View(model);
        }
    }
}