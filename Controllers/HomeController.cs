using Microsoft.AspNetCore.Mvc;

namespace ObituariosWeb.Controllers
{
    public class HomeController : Controller
    {
     public IActionResult Index()
{
    return View();
}
    }
}