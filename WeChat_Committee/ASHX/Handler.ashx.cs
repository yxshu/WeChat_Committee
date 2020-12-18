using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat_Committee.Model;
using WeChat_Committee.Uitl;

namespace WeChat_Committee.ASHX
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            bool configURLResult;
            string echoString = Common.ConfigURL(context, out configURLResult);
            if (configURLResult)
            {
                context.Response.Write(echoString);
            }
            context.Response.Flush();
        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}