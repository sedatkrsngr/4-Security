using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSRF.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

  
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllersWithViews();//eskisi
            services.AddControllersWithViews(opts=> {

                opts.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());// bunu kullan�rsa art�k t�m uygulamada �al��t�r�r ve methodlar�n �zerinde  [ValidateAntiForgeryToken]  koymaya gerek kalm�yor ama baz� post methodlar�nda bunu istemiyorsak [IgnoreAntiForgeryToken] kullan�r�z
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=CommentAdd}/{id?}");
            });
        }
    }
}
