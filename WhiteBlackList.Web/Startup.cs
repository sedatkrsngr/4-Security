using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhiteBlackList.Web.Filters;
using WhiteBlackList.Web.MiddleWares;

namespace WhiteBlackList.Web
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
            services.AddScoped<CheckWhiteList>(); //controller metot seviyesinde filter verirken consructor Iplist istediðinden dolayý servis olarak çalýþtýrýrýz ve [CheckWhiteList(..)] yerine [ServiceFilter(typeof(CheckWhiteList))] kullanarak hangi metotta sorgulama yapsýn belirtebiliriz

            services.Configure<IPList>(Configuration.GetSection("IPList")); //appsettings.json içersindeki deðerlerle ayný adý oluþturan classlara doldur dedik.

            services.AddControllersWithViews();
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

            app.UseMiddleware<IPSafeMiddleWare>(); //Burada oluþturduðumuz middleware çalýþsýn dedik

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
