using Microsoft.AspNetCore.Mvc;

namespace E_commerce_project.Controllers.Admin
{
    public class AdminProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
