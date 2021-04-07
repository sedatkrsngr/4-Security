using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WhiteBlackList.Web.MiddleWares
{
    public class IPSafeMiddleWare //Uygulama bazında Engelleme
    {
        public readonly RequestDelegate _next;//gelen isteğin bilgileri burada tutulur
        public readonly IPList _ipList;//WhiteList tutan sınıfımız

        public IPSafeMiddleWare(RequestDelegate next, IOptions<IPList> ipList)
        {
            _next = next;
            _ipList = ipList.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var istekIpAdress = context.Connection.RemoteIpAddress;

            var isWhiteList = _ipList.WhiteList.Where(x=> IPAddress.Parse(x).Equals(istekIpAdress)).Any();//Gelen istek beyaz liste içerisinde var mı yok mu sorgusu

            if (!isWhiteList)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;//Bu sayfaya erişim yasak 403 hatası verir
                return;
            }

            await _next(context);//Hata yoksa devam etsin dedik
        }
    }
}
