using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenRedirectAttacks.Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index(string text)
        {
          //  var product = _context.Product.FromSqlRaw("select *from product where Name="+"'"+text+"'").ToList();//böyle bir sorgu sql sorgusu gelirse soruna yol açar
          //  var product = _context.Product.FromSqlRaw("select *from product where Name={0}",text).ToList();//böyle bir sorgu sql sorgusu gelirse soruna yol açmaz
          //  var product = _context.Product.FromSqlInterpolated($"select *from product where Name={text}).ToList();//böyle bir sorgu sql sorgusu gelirse soruna yol açmaz

            return View();
        }
    }
}
