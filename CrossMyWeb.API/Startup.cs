using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossMyWeb.API
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
            services.AddCors(opt=> {

                //opt.AddDefaultPolicy(builder=> {//varsayýlan herkese açmak istersek bunu kullanýrýz
                //    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();//herhangi origin(hangi websitesinden) adresinden,headerinde ve metotundan gelirse gelsin izin ver dedik
                //});

                //opt.AddPolicy("AllowSite",buider=> {

                //    buider.WithOrigins("https://localhost:44335","https://www.google.com").AllowAnyHeader().AllowAnyMethod();//Web Uygulamam ve googledan gelen istekleri header ve metod ne olursa olsun kabul et dedim.
                //});

                opt.AddPolicy("AllowSite2", buider => {

                    buider.WithOrigins("https://localhost:44335").WithHeaders(HeaderNames.ContentType,"x-custom-header").AllowAnyMethod();//ilgili adresten gelen istekleri header içinde belirtilen alan olmasý gerekiyor
                });

                //opt.AddPolicy("AllowSite3", buider => {

                //    buider.WithOrigins("https://*.site.com").SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyHeader.AllowAnyMethod();//* ise örne mobile.site.com, www.site.com gibi sublarýn hepsine uygulanmak için kullanýlabilir.
                //});


                opt.AddPolicy("AllowSite4", buider => {

                    buider.WithOrigins("https://localhost:44335").WithMethods("POST","GET").AllowAnyHeader();
                });

            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CrossMyWeb.API", Version = "v1" });
            });
        }

       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CrossMyWeb.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseCors();//bunun yeri  burasý olmalý  Routing ve aut arasý. Isim vermezsek servicesteki defaultpolicy çalýþýr 

            //app.UseCors("AllowSite");//AllowSite için baþlatýr
           app.UseCors();//Controller method bazýnda cors kullanacaksak böyle default yaparýz ardýndan controllere gideriz.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
