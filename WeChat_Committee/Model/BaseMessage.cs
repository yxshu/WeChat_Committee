using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WeChat_Committee.Uitl;

namespace WeChat_Committee.Model
{
    /// <summary>
    /// 消息体基类
    /// 
    /// 一旦遇到以下情况，微信都会在公众号会话中，向用户下发系统提示“该公众号暂时无法提供服务，请稍后再试”
    /// 1、开发者在5秒内未回复任何内容
    ///2、开发者回复了异常数据，比如JSON数据等
    ///
    /// 消息基类BaseMessage，而不管我们接收到什么类型的消息，都需要可以调用方法，进行响应用户的请求，
    /// 所以，用户回复用户请求的方法需要封装到基类中
    /// </summary>
    public abstract class BaseMessage
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public MsgType MsgType { get; set; }

        public virtual void ResponseNull()
        {
            Common.ResponseWrite("");
        }
        /// <summary>
        /// 回复文本消息
        /// <xml>
        ///<ToUserName><![CDATA[接收方帐号（收到的OpenID）]]></ToUserName>
        ///<FromUserName><![CDATA[开发者微信号]]></FromUserName>
        ///<CreateTime>消息创建时间 （整型）</CreateTime>
        ///<MsgType><![CDATA[image]]></MsgType>
        ///<Content><![CDATA[回复的消息内容（换行：在content中能够换行，微信客户端就支持换行显示）]]></Content>
        ///</xml>
        /// </summary>
        /// <param name="param"></param>
        /// <param name="content"></param>
        public virtual void ResText(EnterParam param, string content)
        {
            StringBuilder resxml = new StringBuilder(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime>", FromUserName, ToUserName, Common.ConvertDateTimeInt(DateTime.Now)));
            resxml.AppendFormat("<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[{0}]]></Content><FuncFlag>0</FuncFlag></xml>", content);
            Response(param, resxml.ToString());
        }
        /// <summary>
        /// 回复消息(音乐)
        //<xml>
        //<ToUserName><![CDATA[接收方帐号（收到的OpenID）]]></ToUserName>
        //<FromUserName><![CDATA[开发者微信号]]></FromUserName>
        //<CreateTime>消息创建时间 （整型）</CreateTime>
        //<MsgType><![CDATA[music]]></MsgType>
        //<Music>
        //<ThumbMediaId><![CDATA[缩略图的媒体id, 通过上传多媒体文件，得到的id。]]></ThumbMediaId>
        //<Title><![CDATA[视频消息的标题]]></Title> 
        //<Description><![CDATA[视频消息的描述]]></Description> 
        //<MusicURL><![CDATA[音乐链接]]></MusicURL> 
        //<HQMusicUrl><![CDATA[高质量音乐链接，WIFI环境优先使用该链接播放音乐]]></HQMusicUrl> 
        //</Music>
        //</xml>
        /// </summary>
        public void ResMusic(EnterParam param, Music mu)
        {
            StringBuilder resxml = new StringBuilder(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime>", FromUserName, ToUserName, Common.ConvertDateTimeInt(DateTime.Now)));
            resxml.Append(" <MsgType><![CDATA[music]]></MsgType>");
            resxml.AppendFormat("<Music><Title><![CDATA[{0}]]></Title><Description><![CDATA[{1}]]></Description>", mu.Title, mu.Description);
            resxml.AppendFormat("<MusicUrl><![CDATA[http://{0}{1}]]></MusicUrl><HQMusicUrl><![CDATA[http://{2}{3}]]></HQMusicUrl></Music><FuncFlag>0</FuncFlag></xml>", VqiRequest.GetCurrentFullHost(), mu.MusicUrl, VqiRequest.GetCurrentFullHost(), mu.HQMusicUrl);
            Response(param, resxml.ToString());
        }
        /// <summary>
        /// 回复视频消息
        //<xml>
        //<ToUserName><![CDATA[接收方帐号（收到的OpenID）]]></ToUserName>
        //<FromUserName><![CDATA[开发者微信号]]></FromUserName>
        //<CreateTime>消息创建时间 （整型）</CreateTime>
        //<MsgType><![CDATA[video]]></MsgType>
        //<Video>
        //<MediaId><![CDATA[通过上传多媒体文件，得到的id。]]></MediaId>
        //<Title><![CDATA[视频消息的标题]]></Title> 
        //<Description><![CDATA[视频消息的描述]]></Description> 
        //</Video>
        //</xml>
        /// </summary>
        /// <param name="param"></param>
        /// <param name="v"></param>
        public void ResVideo(EnterParam param, Video v)
        {
            StringBuilder resxml = new StringBuilder(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime>", FromUserName, ToUserName, Common.ConvertDateTimeInt(DateTime.Now)));
            resxml.Append(" <MsgType><![CDATA[video]]></MsgType>");
            resxml.AppendFormat("<Video><MediaId><![CDATA[{0}]]></MediaId>", v.media_id);
            resxml.AppendFormat("<Title><![CDATA[{0}]]></Title>", v.title);
            resxml.AppendFormat("<Description><![CDATA[{0}]]></Description></Video></xml>", v.description);
            Response(param, resxml.ToString());
        }

        /// <summary>
        /// 回复消息(图片)
        //<xml>
        //<ToUserName><![CDATA[接收方帐号（收到的OpenID）]]></ToUserName>
        //<FromUserName><![CDATA[开发者微信号]]></FromUserName>
        //<CreateTime>消息创建时间 （整型）</CreateTime>
        //<MsgType><![CDATA[image]]></MsgType>
        //<Image>
        //<MediaId><![CDATA[通过上传多媒体文件，得到的id。]]></MediaId>
        //</Image>
        //</xml>
        /// </summary>
        public void ResPicture(EnterParam param, Picture pic, string domain)
        {
            StringBuilder resxml = new StringBuilder(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime>", FromUserName, ToUserName, Common.ConvertDateTimeInt(DateTime.Now)));
            resxml.Append(" <MsgType><![CDATA[image]]></MsgType>");
            resxml.AppendFormat("<PicUrl><![CDATA[{0}]]></PicUrl></xml>", domain + pic.PictureUrl);
            Response(param, resxml.ToString());
        }

        /// <summary>
        /// 回复消息（图文列表）
        /// item是一个项，一个item代码一个图文。在响应的时候，我们只需根据数据格式，替换掉对应的属性，然后Response.Write(s)即可
        //<xml>
        //<ToUserName><![CDATA[toUser]]></ToUserName>
        //<FromUserName><![CDATA[fromUser]]></FromUserName>
        //<CreateTime>12345678</CreateTime>
        //<MsgType><![CDATA[news]]></MsgType>
        //<ArticleCount>2</ArticleCount>
        //<Articles>
        //<item>
        //<Title><![CDATA[title1]]></Title> 
        //<Description><![CDATA[description1]]></Description>
        //<PicUrl><![CDATA[picurl]]></PicUrl>
        //<Url><![CDATA[url]]></Url>
        //</item>
        //<item>
        //<Title><![CDATA[title]]></Title>
        //<Description><![CDATA[description]]></Description>
        //<PicUrl><![CDATA[picurl]]></PicUrl>
        //<Url><![CDATA[url]]></Url>
        //</item>
        //</Articles>
        //</xml>
        /// </summary>
        /// <param name="param"></param>
        /// <param name="art"></param>
        public void ResArticles(EnterParam param, List<Articles> art)
        {
            StringBuilder resxml = new StringBuilder(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime>", FromUserName, ToUserName, Common.ConvertDateTimeInt(DateTime.Now)));
            resxml.AppendFormat("<MsgType><![CDATA[news]]></MsgType><ArticleCount>{0}</ArticleCount><Articles>", art.Count);
            for (int i = 0; i < art.Count; i++)
            {
                resxml.AppendFormat("<item><Title><![CDATA[{0}]]></Title>  <Description><![CDATA[{1}]]></Description>", art[i].Title, art[i].Description);
                resxml.AppendFormat("<PicUrl><![CDATA[{0}]]></PicUrl><Url><![CDATA[{1}]]></Url></item>", art[i].PicUrl.Contains("http://") ? art[i].PicUrl : "http://" + VqiRequest.GetCurrentFullHost() + art[i].PicUrl, art[i].Url.Contains("http://") ? art[i].Url : "http://" + VqiRequest.GetCurrentFullHost() + art[i].Url);
            }
            resxml.Append("</Articles><FuncFlag>0</FuncFlag></xml>");
            Response(param, resxml.ToString());
        }
        /// <summary>
        /// 多客服转发
        /// </summary>
        /// <param name="param"></param>
        public void ResDKF(EnterParam param)
        {
            StringBuilder resxml = new StringBuilder();
            resxml.AppendFormat("<xml><ToUserName><![CDATA[{0}]]></ToUserName>", FromUserName);
            resxml.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName><CreateTime>{1}</CreateTime>", ToUserName, CreateTime);
            resxml.AppendFormat("<MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>");
            Response(param, resxml.ToString());
        }
        /// <summary>
        /// 多客服转发如果指定的客服没有接入能力(不在线、没有开启自动接入或者自动接入已满)，该用户会一直等待指定客服有接入能力后才会被接入，而不会被其他客服接待。建议在指定客服时，先查询客服的接入能力指定到有能力接入的客服，保证客户能够及时得到服务。
        /// </summary>
        /// <param name="param">用户发送的消息体</param>
        /// <param name="KfAccount">多客服账号</param>
        public void ResDKF(EnterParam param, string KfAccount)
        {
            StringBuilder resxml = new StringBuilder();
            resxml.AppendFormat("<xml><ToUserName><![CDATA[{0}]]></ToUserName>", FromUserName);
            resxml.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName><CreateTime>{1}</CreateTime>", ToUserName, CreateTime);
            resxml.AppendFormat("<MsgType><![CDATA[transfer_customer_service]]></MsgType><TransInfo><KfAccount>{0}</KfAccount></TransInfo></xml>", KfAccount);
            Response(param, resxml.ToString());
        }
        private void Response(EnterParam param, string data)
        {
            if (param.IsAes)
            {
                var wxcpt = new MsgCrypt(param.token, param.EncodingAESKey, param.appid);
                wxcpt.EncryptMsg(data, Common.ConvertDateTimeInt(DateTime.Now).ToString(), Utils.GetRamCode(), ref data);
            }
            Common.ResponseWrite(data);

        }
    }


    /// <summary>
    /// 微信接入参数
    /// </summary>
    public class EnterParam
    {
        /// <summary>
        /// 是否加密
        /// </summary>
        public bool IsAes { get; set; }
        /// <summary>
        /// 接入token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        ///微信appid
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 加密密钥
        /// </summary>
        public string EncodingAESKey { get; set; }
    }
}