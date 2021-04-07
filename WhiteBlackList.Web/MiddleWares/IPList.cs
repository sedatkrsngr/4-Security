using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhiteBlackList.Web.MiddleWares
{
    public class IPList// json adı ile aynı olmalı
    {
        public string[] WhiteList { get; set; }
    }
}
