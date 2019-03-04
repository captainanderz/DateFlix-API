using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DateflixMVC.Controllers
{
    public class PrivateChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}