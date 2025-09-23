using Microsoft.AspNetCore.Mvc;

namespace E_commerce_project.Controllers
{
    public class CategoryManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
