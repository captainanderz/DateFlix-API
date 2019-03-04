using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dateflix.Pages
{
    public class IndexModel : PageModel
    {
        public string TestProperty { get; set; }
        public void OnGet()
        {
            TestProperty = "This have valuueee!";
        }
    }
}
