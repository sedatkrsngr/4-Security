using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using XSS.Web.Models;

namespace XSS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private HtmlEncoder _htmlEncoder;
        private JavaScriptEncoder _javaScriptEncoder;
        private UrlEncoder _urlEncoder;

        public HomeController(ILogger<HomeController> logger, HtmlEncoder htmlEncoder, JavaScriptEncoder javaScriptEncoder, UrlEncoder urlEncoder)
        {
            _logger = logger;
            _htmlEncoder = htmlEncoder;
            _javaScriptEncoder = javaScriptEncoder;
            _urlEncoder = urlEncoder;
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
        [HttpPost]
        public IActionResult CommentAdd(string name,string comment)
        {
           string encodeName= _urlEncoder.Encode(name);//urlde zararsız hale gelir

           string encodeCommentHtml= _htmlEncoder.Encode(comment);//Texteditor kullanıyorsak bunu kullanırsak temiz güvenli olur

            string encodeCommentJavascript = _javaScriptEncoder.Encode(comment);//commentten gelen javascript kodunu güvenli hale getirir

            ViewBag.Name = name;
            ViewBag.Comment = comment;

            System.IO.File.AppendAllText("comment.txt",$"{name}-{comment}\n");
            return RedirectToAction("CommentAdd");
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
