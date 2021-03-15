using app.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace app.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        /// <summary>
        /// Home page
        /// </summary>
        /// <returns>home index view</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Error page
        /// </summary>
        /// <returns>error view</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
