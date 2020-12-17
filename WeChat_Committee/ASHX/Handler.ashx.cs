using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat_Committee.ASHX
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string token = "yxshu";
            if (string.IsNullOrEmpty(token)) { return; }
            string echoString = HttpContext.Current.Request.QueryString["echoStr"];
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];
            if (checkSignature(token, signature, timestamp, nonce))
            {
                if (!string.IsNullOrEmpty(echoString))
                {
                    context.Response.Write(echoString);
                    context.Response.End();
                }
            }
        }

        /// <summary>
        /// 验证微信签名
        /// 
        /// 1）将token、timestamp、nonce三个参数进行字典序排序 
        /// 2）将三个参数字符串拼接成一个字符串进行sha1加密 
        /// 3）开发者获得加密后的字符串可与signature对比，标识该请求来源于微信
        /// </summary>
        /// <param name="token"></param>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        private bool checkSignature(string token, string signature, string timestamp, string nonce)
        {
            string[] ArrTmp = { token, timestamp, nonce };
            //字典排序
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1").ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else { return false; }
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