using Microsoft.AspNetCore.Mvc;

namespace Sales.Controllers
{
    public class DeliveryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}