using CSRF.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CSRF.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult CommentAdd()
        {
            HttpContext.Response.Cookies.Append("email", "deneme@hotm.com");//Örn cookiye bilgilerini html.raw yüzünden javascript ile çekebilirler
            HttpContext.Response.Cookies.Append("password", "1234");

            if (System.IO.File.Exists("comment.txt"))//Html.Raw yüzünden veritabanının içerisine javascript kodu kaydederek herseferinde veriyi çektiğimizde javascript kodu çalışır
            {
                ViewBag.Comments = System.IO.File.ReadAllLines("comment.txt");
            }

            return View();
        }
        [ValidateAntiForgeryToken] //post isteğine gelen istekle post isteğinin tokenları aynı ise arka tarafta işlem gerçekleşir get tarafında gizli bir token oluşur. Method bazlı böyle kullanılır
        [HttpPost]
        public IActionResult CommentAdd(string name, string comment)
        {
            

            ViewBag.Name = name;
            ViewBag.Comment = comment;

            System.IO.File.AppendAllText("comment.txt", $"{name}-{comment}\n");
            return RedirectToAction("CommentAdd");
        }
        public IActionResult Index()
        {
            return View();
        }

        [IgnoreAntiforgeryToken]
        [HttpPost]
        public IActionResult Index2()
        {
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
