using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;
using WeChat_Committee.Model;

namespace WeChat_Committee.Uitl
{
    public class Common
    {

        /// <summary>
        /// 微信上传多媒体文件
        /// 多媒体文件、多媒体消息的获取和调用等操作，是通过media_id来进行的。
        /// 通过本接口，公众号可以上传或下载多媒体文件。
        /// 但请注意，每个多媒体文件（media_id）会在上传、用户发送到微信服务器3天后自动删除，以节省服务器资源。
        /// 上传多媒体的接口地址是：http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=ACCESS_TOKEN&type=TYPE
        /// 其中access_token为调用接口凭证，type是媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）
        /// 上传的多媒体文件有格式和大小限制，如下：

        ///图片（image）: 1M，支持JPG格式
        ///语音（voice）：2M，播放长度不超过60s，支持AMR\MP3格式
        ///视频（video）：10MB，支持MP4格式
        ///缩略图（thumb）：64KB，支持JPG格式
        /// </summary>
        /// <param name="filepath">文件绝对路径</param>
        public static UpLoadedMediaInfo WxUpLoad(string filepath, string token, MediaType mt)
        {
            using (WebClient client = new WebClient())
            {
                byte[] b = client.UploadFile(string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", token, mt.ToString()), filepath);//调用接口上传文件
                string retdata = Encoding.Default.GetString(b);//获取返回值
                if (retdata.Contains("media_id"))//判断返回值是否包含media_id，包含则说明上传成功，然后将返回的json字符串转换成json
                {
                    return JsonConvert.DeserializeObject<UpLoadedMediaInfo>(retdata);
                }
                else
                {//否则，写错误日志

                    WriteBug(retdata);//写错误日志
                    return null;
                }
            }
        }

        private static void WriteBug(string exceptionmessage)
        {
            string path = string.Empty;
            try
            {
                path = ConfigurationManager.AppSettings["Logpath"];
            }
            catch (Exception)
            {
                path = @"D:\Logs";
            }
            if (string.IsNullOrEmpty(path))
            { path = @"D:\Logs"; }
            try
            {
                //如果日志目录不存在，则创建该目录
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string logFileName = path + "\\程序日志_" + DateTime.Now.ToString("yyyy_MM_dd_HH") + ".log";
                StringBuilder logContents = new StringBuilder();
                logContents.AppendLine(exceptionmessage);
                //当天的日志文件不存在则新建，否则追加内容
                StreamWriter sw = new StreamWriter(logFileName, true, Encoding.Unicode);
                sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:sss") + "" + logContents.ToString());
                sw.Flush();
                sw.Close();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 数据包（如加密，需解密后传入），以基类的形式返回对应的实体。
        /// </summary>
        /// <param name="param"></param>
        /// <param name="bug"></param>
        /// <returns></returns>
        public static BaseMessage Load(EnterParam param, bool bug = true)
        {
            string postStr = "";
            Stream s = VqiRequest.GetInputStream();//此方法是对System.Web.HttpContext.Current.Request.InputStream的封装，可直接代码
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            postStr = Encoding.UTF8.GetString(b);//获取微信服务器推送过来的字符串
            ///然后再分别获取url中的参数：timestamp，nonce，msg_signature，encrypt_type。可以看到，在明文模式下是没有encrypt_type参数的。
            ///兼容模式和安全模式则加入了消息体的签名与加密类型两个参数。
            ///由于在实际的运营中，兼容模式不太可能使用，所以在此不做详细介绍了。
            ///获取到url中的参数后，判断encrypt_type的值是否为aes，如果是则说明是使用的兼容模式或安全模式，此时则需调用解密相关的方法进行解密。
            ///则则直接解析接收到的xml字符串。
            var timestamp = VqiRequest.GetQueryString("timestamp");
            var nonce = VqiRequest.GetQueryString("nonce");
            var msg_signature = VqiRequest.GetQueryString("msg_signature");
            var encrypt_type = VqiRequest.GetQueryString("encrypt_type");
            string data = "";
            if (encrypt_type == "aes")//加密模式处理
            {
                param.IsAes = true;
                var ret = new MsgCrypt(param.token, param.EncodingAESKey, param.appid);
                int r = ret.DecryptMsg(msg_signature, timestamp, nonce, postStr, ref data);
                if (r != 0)
                {
                    WriteBug("消息解密失败");
                    return null;

                }
            }
            else
            {
                param.IsAes = false;
                data = postStr;
            }
            if (bug)
            {
               WriteBug(data);
            }
            return MessageFactory.CreateMessage(data);
        }


        /// <summary>
        /// 根据微信服务器推送的消息体解析成对应的实体。
        /// 本打算用C#自带的xml序列化发序列化的组件，结果试了下总是报什么xmls的错，
        /// 索性用反射写了个处理方法：
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static T XMLConvertObj<T>(string xmlstr)
        {
            XElement xdoc = XElement.Parse(xmlstr);
            var type = typeof(T);
            var t = Activator.CreateInstance<T>();
            foreach (XElement element in xdoc.Elements())
            {
                var pr = type.GetProperty(element.Name.ToString());
                if (element.HasElements)
                {//这里主要是兼容微信新添加的菜单类型。nnd，竟然有子属性，所以这里就做了个子属性的处理
                    foreach (var ele in element.Elements())
                    {
                        pr = type.GetProperty(ele.Name.ToString());
                        pr.SetValue(t, Convert.ChangeType(ele.Value, pr.PropertyType), null);
                    }
                    continue;
                }
                if (pr.PropertyType.Name == "MsgType")//获取消息模型
                {
                    pr.SetValue(t, (MsgType)Enum.Parse(typeof(MsgType), element.Value.ToUpper()), null);
                    continue;
                }
                if (pr.PropertyType.Name == "Event")//获取事件类型。
                {
                    pr.SetValue(t, (Event)Enum.Parse(typeof(Event), element.Value.ToUpper()), null);
                    continue;
                }
                pr.SetValue(t, Convert.ChangeType(element.Value, pr.PropertyType), null);
            }
            return t;
        }

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
            ///https://www.cnblogs.com/zskbll/p/4164079.html
        }
        /// <summary>
        /// 首先从数据库中查询最近的access_token,如果没有过期（有效期超过5分钟为准），则直接返回
        /// 如果不存在，或者已经过期，则重新申请access_token并将其写入数据库
        /// 
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
            DataSet dataSet = DBHelper.DbHelperSQL.Query("select top(1)* from Token order by createtime desc");
            Token token = null;
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                token = GeneralUtil.GetModelByDataRow<Token>(dataSet.Tables[0].Rows[0]);
                //token = new Token();
                //DataRow dr = dataSet.Tables[0].Rows[0];
                //token.Id = (Int32)dr["Id"];
                //token.Access_token = dr["access_token"].ToString();
                //token.Expires_in = (Int32)dr["expires_in"];
                //token.Createtime = (DateTime)dr["createtime"];
            }
            if (token != null && DateTime.Now < token.Createtime.AddSeconds(token.Expires_in + 300)) return token;
            string grant_type = ConfigurationManager.AppSettings["grant_type"];
            string APPID = ConfigurationManager.AppSettings["appid"];
            string APPSECRET = ConfigurationManager.AppSettings["APPSECRET"];
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=" + grant_type + "&appid=" + APPID + "&secret=" + APPSECRET;
            HttpWebResponse httpWebResponse = HttpHelper.CreateGetHttpResponse(url, 5 * 1000, null, null);
            string responsestring = HttpHelper.GetResponseString(httpWebResponse);
            JsonSerializer jsonSerializer = new JsonSerializer();
            token = (Token)jsonSerializer.Deserialize(new JsonTextReader(new System.IO.StringReader(responsestring)), typeof(Token));
            SqlParameter[] sqlparameters = {
                new SqlParameter("@access_token",System.Data.SqlDbType.Text),
                new SqlParameter("@expires_in",System.Data.SqlDbType.Int),
                new SqlParameter("@createtime",System.Data.SqlDbType.DateTime)
            };
            sqlparameters[0].Value = token.Access_token;
            sqlparameters[1].Value = token.Expires_in;
            sqlparameters[2].Value = token.Createtime;
            StringBuilder sql = new StringBuilder("insert into Token(access_token,expires_in,createtime) ");
            sql.Append("values(@access_token,@expires_in,@createtime)");
            int rows = DBHelper.DbHelperSQL.ExecuteSql(sql.ToString(), sqlparameters);
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

        public static void ResponseWrite(string str)
        {
            HttpContext.Current.Response.Write(str);
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 表示1970年1月1日0时0分0秒至输入时间所间隔的秒数
        /// </summary>
        /// <param name="datatime"></param>
        /// <returns></returns>
        public static long ConvertDateTimeInt(DateTime datatime)
        {
            DateTime start = new DateTime(1970, 1, 1);
            long totalseconds = (long)(datatime - start).TotalSeconds;
            return totalseconds;
        }
    }
}