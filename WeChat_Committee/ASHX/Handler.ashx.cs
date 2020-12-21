using System.Web;
using System.Web.SessionState;
using WeChat_Committee.Model;
using WeChat_Committee.Uitl;

namespace WeChat_Committee.ASHX
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            ///查询access_token
            Token token = Common.getAccess_Token();
            context.Response.Write(token.ToString());

           // ///根据请求方式决定使用的函数
           //string requesttype = context.Request.RequestType.ToUpper();
           // switch (requesttype)
           // {
           //     case "GET": return;
           //     case "POST": return;
           // }

           // ///微信公众号服务器接入验证
           // bool configURLResult;
           // string echoString = Common.ConfigURL(context, out configURLResult);
           // if (configURLResult)
           // {
           //     context.Response.Write(echoString);
           // }
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