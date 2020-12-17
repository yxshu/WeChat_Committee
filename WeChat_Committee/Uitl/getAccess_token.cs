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
        public Token getToken()
        {
            string grant_type = ConfigurationManager.AppSettings["grant_type"];
            string APPID = ConfigurationManager.AppSettings["appid"];
            string APPSECRET = ConfigurationManager.AppSettings["APPSECRET"];
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=" + grant_type + "&appid=" + APPID + "&secret=" + APPSECRET;
            HttpWebResponse httpWebResponse = HttpHelper.CreateGetHttpResponse(url, 5 * 1000, null, null);
            string responsestring = HttpHelper.GetResponseString(httpWebResponse);
            JsonSerializer jsonSerializer = new JsonSerializer();
            Token token = (Token)jsonSerializer.Deserialize(new JsonTextReader(new System.IO.StringReader(responsestring)), typeof(Token));
            //bool tokenInsertResult = SetAppSettingsValue("access_token", responsestring);
            return token;
        }
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