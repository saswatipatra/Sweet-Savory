using Microsoft.AspNetCore.Mvc;
using SweetSavory.Models;

namespace SweetSavory.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }
    }
}