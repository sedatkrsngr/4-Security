using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WhiteBlackList.Web.MiddleWares;

namespace WhiteBlackList.Web.Filters
{
    public class CheckWhiteList :ActionFilterAttribute//Controller ve Method seviyesine yakalamk için
    {
        public readonly IPList _ipList;//WhiteList tutan sınıfımız

        public CheckWhiteList(IOptions<IPList> ipList)
        {
            _ipList = ipList.Value;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var istekIpAdress = context.HttpContext.Connection.RemoteIpAddress;

            var isWhiteList = _ipList.WhiteList.Where(x => IPAddress.Parse(x).Equals(istekIpAdress)).Any();//Gelen istek beyaz liste içerisinde var mı yok mu sorgusu

            if (!isWhiteList)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);//eğer metodda whitelistte değilse istek yapan 403 verir                
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
