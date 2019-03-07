using System.Diagnostics;
using System.Linq;
using DateflixMVC.Hubs;
using DateflixMVC.Models;
using DateflixMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DateflixMVC.Controllers
{
    public class LoginController : Controller
    {
        private IUserService _userService;
        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Test()
        {

            return View();
        }

        public IActionResult Login(string username, string password)
        {
            var user = _userService.Authenticate(username, password);

            if (user == null)
            {
                return Content("Error");
            }

            HttpContext.Session.SetString("Username", user.Username);

            return Content("OK");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
