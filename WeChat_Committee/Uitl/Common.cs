using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;
using WeChat_Committee.Model;

namespace WeChat_Committee.Uitl
{
    public class Common
    {

        /// <summary>
        /// 查询自定义菜单
        ///接口调用请求说明
        ///http请求方式: GET（请使用https协议）https://api.weixin.qq.com/cgi-bin/get_current_selfmenu_info?access_token=ACCESS_TOKEN
        /// 本接口将会提供公众号当前使用的自定义菜单的配置，如果公众号是通过API调用设置的菜单，则返回菜单的开发配置，而如果公众号是在公众平台官网通过网站功能发布菜单，则本接口返回运营者设置的菜单配置。
        /// 请注意：
        ///第三方平台开发者可以通过本接口，在旗下公众号将业务授权给你后，立即通过本接口检测公众号的自定义菜单配置，并通过接口再次给公众号设置好自动回复规则，以提升公众号运营者的业务体验。
        ///本接口与自定义菜单查询接口的不同之处在于，本接口无论公众号的接口是如何设置的，都能查询到接口，而自定义菜单查询接口则仅能查询到使用API设置的菜单配置。
        ///认证/未认证的服务号/订阅号，以及接口测试号，均拥有该接口权限。
        ///从第三方平台的公众号登录授权机制上来说，该接口从属于消息与菜单权限集。
        //本接口中返回的图片/语音/视频为临时素材（临时素材每次获取都不同，3天内有效，通过素材管理-获取临时素材接口来获取这些素材），本接口返回的图文消息为永久素材素材（通过素材管理-获取永久素材接口来获取这些素材）。

        /// </summary>
        /// <returns></returns>
        public static void querySelfmenuInfo()
        {

        }
        /// <summary>
        /// access_token是公众号的全局唯一接口调用凭据,
        /// 公众号调用各接口时都需使用access_token。
        /// 开发者需要进行妥善保存。
        /// access_token的存储至少要保留512个字符空间。
        /// access_token的有效期目前为2个小时，需定时刷新，
        /// 重复获取将导致上次获取的access_token失效。
        /// 
        /// 接口调用请求说明
        ///https请求方式: GET https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=APPSECRET
        ///参数：grant_type 	必须  	获取access_token填写client_credential
        ///参数：appid        必须   第三方用户唯一凭证
        ///参数：secret       必须   第三方用户唯一凭证密钥，即appsecret
        ///
        /// 返回说明
        ///正常情况下，微信会返回下述JSON数据包给公众号：
        ///{"access_token":"ACCESS_TOKEN","expires_in":7200}
        ///access_token 	获取到的凭证
        ///expires_in 凭证有效时间，单位：秒
        ///
        /// 错误时微信会返回错误码等信息，JSON数据包示例如下（该示例为AppID无效错误）:
        ///{"errcode":40013,"errmsg":"invalid appid"}

        /// </summary>
        /// <returns></returns>
        public static Token getAccess_Token()
        {
            string grant_type = ConfigurationManager.AppSettings["grant_type"];
            string APPID = ConfigurationManager.AppSettings["appid"];
            string APPSECRET = ConfigurationManager.AppSettings["APPSECRET"];
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=" + grant_type + "&appid=" + APPID + "&secret=" + APPSECRET;
            HttpWebResponse httpWebResponse = HttpHelper.CreateGetHttpResponse(url, 5 * 1000, null, null);
            string responsestring = HttpHelper.GetResponseString(httpWebResponse);
            JsonSerializer jsonSerializer = new JsonSerializer();
            Token token = new Token();
            token = (Token)jsonSerializer.Deserialize(new JsonTextReader(new System.IO.StringReader(responsestring)), typeof(Token));
            
            return token;
        }
        /// <summary>
        /// 配置微信公众号自有服务器地址
        /// 开发者提交信息后，微信服务器将发送GET请求到填写的服务器地址URL上，
        /// 开发者通过检验signature对请求进行校验（下面有校验方式）。
        /// 若确认此次GET请求来自微信服务器，请原样返回echostr参数内容，则接入生效，成为开发者成功，
        /// 否则接入失败。
        /// 校验方法见checkSignature方法
        /// </summary>
        /// <param name="context">微信服务器发送的Get请求上下文对象</param>
        /// <param name="result">输出参数，验证是否成功的标识</param>
        /// <returns>返回一个string，用于回复，成功则按要求返回echoString，不成功则返回null</returns>
        public static string ConfigURL(HttpContext context, out bool result)
        {
            string token = ConfigurationManager.AppSettings["token"];
            result = false;
            if (string.IsNullOrEmpty(token)) { return null; }
            string echoString = context.Request.QueryString["echoStr"];
            string signature = context.Request.QueryString["signature"];
            string timestamp = context.Request.QueryString["timestamp"];
            string nonce = context.Request.QueryString["nonce"];
            if (checkSignature(token, signature, timestamp, nonce))
            {
                if (!string.IsNullOrEmpty(echoString))
                {
                    result = true;
                    return echoString;
                }
            }
            return null;
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
        private static bool checkSignature(string token, string signature, string timestamp, string nonce)
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
        /// <summary>
        /// 向配置文件的AppSetting字段内写入内容
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        private static bool SetAppSettingsValue(string key, string value)
        {
            bool result = false;
            //增加的内容写在appSettings段下 <add key="RegCode" value="0"/>  
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件  
            result = true;
            return result;
        }
    }
}