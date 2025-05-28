using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using POS.Entities;
using POS.Web.Models;

namespace POS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string _userName;
        

        public HomeController(ILogger<HomeController> logger, MySQLiteContext context)
        {
            _logger = logger;
            _userName = string.Empty;
        }

        public IActionResult Index(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                _userName = username;
                ViewData["User"] = _userName;
            }
            else
            {
                ViewData["User"] = "Invitado";
            } 
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
