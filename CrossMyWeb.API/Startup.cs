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

                //opt.AddDefaultPolicy(builder=> {//varsay�lan herkese a�mak istersek bunu kullan�r�z
                //    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();//herhangi origin(hangi websitesinden) adresinden,headerinde ve metotundan gelirse gelsin izin ver dedik
                //});

                //opt.AddPolicy("AllowSite",buider=> {

                //    buider.WithOrigins("https://localhost:44335","https://www.google.com").AllowAnyHeader().AllowAnyMethod();//Web Uygulamam ve googledan gelen istekleri header ve metod ne olursa olsun kabul et dedim.
                //});

                opt.AddPolicy("AllowSite2", buider => {

                    buider.WithOrigins("https://localhost:44335").WithHeaders(HeaderNames.ContentType,"x-custom-header").AllowAnyMethod();//ilgili adresten gelen istekleri header i�inde belirtilen alan olmas� gerekiyor
                });

                //opt.AddPolicy("AllowSite3", buider => {

                //    buider.WithOrigins("https://*.site.com").SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyHeader.AllowAnyMethod();//* ise �rne mobile.site.com, www.site.com gibi sublar�n hepsine uygulanmak i�in kullan�labilir.
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

            // app.UseCors();//bunun yeri  buras� olmal�  Routing ve aut aras�. Isim vermezsek servicesteki defaultpolicy �al���r 

            //app.UseCors("AllowSite");//AllowSite i�in ba�lat�r
           app.UseCors();//Controller method baz�nda cors kullanacaksak b�yle default yapar�z ard�ndan controllere gideriz.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
