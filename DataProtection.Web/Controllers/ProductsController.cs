using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataProtection.Web.Models;
using Microsoft.AspNetCore.DataProtection;

namespace DataProtection.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly SwaggerDBContext _context;
        private readonly IDataProtector _dataProtector;//DataProtector startuptan çalıştırıldı 

        public ProductsController(SwaggerDBContext context,IDataProtectionProvider dataProtectionProvider)
        {
            _context = context;
            _dataProtector = dataProtectionProvider.CreateProtector("ProductSayfa");//buradaki isimle şifreleme yaparız
        }

       
        public async Task<IActionResult> Index()
        {
            // return View(await _context.Products.ToListAsync()); önceki hali

            var timeLimitProtector = _dataProtector.ToTimeLimitedDataProtector();//şifreleme işlemine bir süre içerisinde kullanım ömrü veririz. Token tarzında düşünebiliriz.

            var product = await _context.Products.ToListAsync();
            product.ForEach(p=> {
                p.sifrelenmisId = timeLimitProtector.Protect(p.Id.ToString(),TimeSpan.FromSeconds(5));// zamanlı protector kullanmadan önce _dataProtector.Protect(p.Id.ToString()) şeklinde süresiz erişebiliyorduk fakat şimdi süreli oldu ve 5 saniye içerisinde kullanılmalı
            });
            //sifrelenmisId alanını doldurarak detay sayfasında çağrılmasını sağladık

            return View(product);

        }

        public async Task<IActionResult> Details(string  id) //int ? id yerine artık string sifrelenmiş Id gelecek
        {
            var timeLimitProtector = _dataProtector.ToTimeLimitedDataProtector();

            int? _id = Convert.ToInt32(timeLimitProtector.Unprotect(id));//Burada şifrelenmiş datayı tekrar eski haline getirerek güvenli bir şekilde işlem yaparız_dataProtector.Unprotect(id) yerine süreli kullanınca böyle çalıştırırız

            if (_id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == _id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Date,Category")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Date,Category")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
