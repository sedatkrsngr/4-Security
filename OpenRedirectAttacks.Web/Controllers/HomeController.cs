using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenRedirectAttacks.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OpenRedirectAttacks.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Login(string returnUrl="/")//default olarak anasayfaya gitmesi için koyduk
        {
            TempData["returnUrl"] = returnUrl;//Post işlemi olduğunda aynı metodun post işlemi veriyi yakalayabiliyor
            return View();
        }
       
        [HttpPost]
        public IActionResult Login(string email,string password)
        {
            string returnUrl = TempData["returnUrl"].ToString();

            //email ve password kontrolleri falan sağlandıktan sonra

            if (Url.IsLocalUrl(returnUrl))//Yönlendirilecek sayfanın local sayfama ait olup olmadığına göre kontrol yaparak yönlendirme dolandırıcılığını engelleriz
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect("/");//sayfama ait değilse anasayfaya dön
            }
            
        }
        public IActionResult Index()
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
