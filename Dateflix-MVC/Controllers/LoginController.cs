using System.Diagnostics;
using System.Linq;
using DateflixMVC.Hubs;
using DateflixMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DateflixMVC.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Test()
        {

            return View();
        }

        public IActionResult Login(string username)
        {
            if (PrivateChatHub.ConnectedUsers.Any(x => x.UserName != username && username != ""))
            {
                HttpContext.Session.SetString("Username", username);
                return RedirectToAction("Index", "PrivateChat");
            }

            return Content("Error");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
