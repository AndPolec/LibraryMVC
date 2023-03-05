using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Web.Controllers
{
    public class LoanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
