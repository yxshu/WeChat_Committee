using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat_Committee.Model
{
    public class Messages
    {
    }
    /// <summary>
    /// 文本实体：
    /// </summary>
    public class TextMessage : BaseMessage
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public string MsgId { get; set; }
    }

    /// <summary>
    /// 图片实体：
    /// </summary>
    public class ImgMessage : BaseMessage
    {
        /// <summary>
        /// 图片路径
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 媒体ID
        /// </summary>
        public string MediaId { get; set; }
    }

    /// <summary>
    /// 语音实体：
    /// </summary>
    public class VoiceMessage : BaseMessage
    {
        /// <summary>
        /// 缩略图ID
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 格式
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 媒体ID
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 语音识别结果
        /// </summary>
        public string Recognition { get; set; }
    }

    /// <summary>
    /// 视频实体：
    /// </summary>
    public class VideoMessage : BaseMessage
    {
        /// <summary>
        /// 缩略图ID
        /// </summary>
        public string ThumbMediaId { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 媒体ID
        /// </summary>
        public string MediaId { get; set; }
    }

    /// <summary>
    /// 链接实体：
    /// </summary>
    public class LinkMessage : BaseMessage
    {
        /// <summary>
        /// 缩略图ID
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// BaseMessage.ResMusic(EnterParam param, Music mu)方法中的Music类
    /// </summary>
    public class Music
    {
        #region 属性
        /// <summary>
        /// 音乐链接
        /// </summary>
        public string MusicUrl { get; set; }
        /// <summary>
        /// 高质量音乐链接，WIFI环境优先使用该链接播放音乐
        /// </summary>
        public string HQMusicUrl { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        #endregion
    }

    /// <summary>
    /// BaseMessage.ResVideo(EnterParam param, Video v)方法中的Video类
    /// </summary>
    public class Video
    {
        public string title { get; set; }
        public string media_id { get; set; }
        public string description { get; set; }
    }

    /// <summary>
    /// BaseMessage.ResArticles(EnterParam param, List<Articles> art)中的Articles类
    /// </summary>
    public class Articles
    {
        #region 属性
        /// <summary>
        /// 图文消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 图文消息描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 图片链接，支持JPG、PNG格式，较好的效果为大图640*320，小图80*80。
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 点击图文消息跳转链接
        /// </summary>
        public string Url { get; set; }
        #endregion
    }
    /// <summary>
    /// 关注/取消关注事件
    /// 订阅/取消订阅事件
    /// 当用户扫描带参数的二维码时，如果用户没有关注当前公众号，用户关注时，会在消息体中带上qrscene_参数，和Ticket，所以这里定义了两个属性：EventKey，Ticket。
    /// 当给EventKey赋值时，替换掉qrscene_，因为我们真正需要的就是后面的参数。
    /// </summary>
    /*
     <xml>
    <ToUserName><![CDATA[toUser]]></ToUserName>
    <FromUserName><![CDATA[FromUser]]></FromUserName>
    <CreateTime>123456789</CreateTime>
    <MsgType><![CDATA[event]]></MsgType>
    <Event><![CDATA[subscribe]]></Event>
    </xml>
     */
    public class SubEventMessage : BaseMessage
    {
        private string _eventkey;
        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值（已去掉前缀，可以直接使用）
        /// </summary>
        public string EventKey
        {
            get { return _eventkey; }
            set { _eventkey = value.Replace("qrscene_", ""); }
        }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
    }

    /// <summary>
    /// 扫描带参数的二维码实体
    /// 扫描带参数二维码事件
    ///用户扫描带场景值二维码时，可能推送一下两种事件：
    ///如果用户还未关注公众号，则用户可以关注公众号，关注后微信会将带场景值关注事件推送给开发者。
    ///如果用户已经关注公众号，则微信会将带场景值扫描事件推送给开发者。、
    /// </summary>
     /*
     用户已关注时的事件推送
    <xml>
    <ToUserName><![CDATA[toUser]]></ToUserName>
    <FromUserName><![CDATA[FromUser]]></FromUserName>
    <CreateTime>123456789</CreateTime>
    <MsgType><![CDATA[event]]></MsgType>
    <Event><![CDATA[SCAN]]></Event>
    <EventKey><![CDATA[SCENE_VALUE]]></EventKey>
    <Ticket><![CDATA[TICKET]]></Ticket>
    </xml>
     */
    public class ScanEventMessage : BaseMessage
    {

        /// <summary>
        /// 事件KEY值，是一个32位无符号整数，即创建二维码时的二维码scene_id
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }

    }

    /// <summary>
    /// 上报地理位置实体
    /// 当公众号开启上报地理位置功能后，每次进入公众号会话时，用户同意上报地理位置后，都会在进入时上报地理位置，
    /// 或在进入回话后每5秒上报一次地理位置，公众号可以再公众平台的后台中修改设置。
    /// 上报地理位置时，微信会将上报地理位置事件推送到开发者填写的url
    /// </summary>
    /*
     * <xml>
    <ToUserName><![CDATA[toUser]]></ToUserName>
    <FromUserName><![CDATA[fromUser]]></FromUserName>
    <CreateTime>123456789</CreateTime>
    <MsgType><![CDATA[event]]></MsgType>
    <Event><![CDATA[LOCATION]]></Event>
    <Latitude>23.137466</Latitude>
    <Longitude>113.352425</Longitude>
    <Precision>119.385040</Precision>
    </xml>
     */
    public class LocationEventMessage : BaseMessage
    {

        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 地理位置精度
        /// </summary>
        public string Precision { get; set; }

    }

    /// <summary>
    /// 普通菜单事件，包括click和view
    /// </summary>
    /*
     <xml>
    <ToUserName><![CDATA[toUser]]></ToUserName>
    <FromUserName><![CDATA[FromUser]]></FromUserName>
    <CreateTime>123456789</CreateTime>
    <MsgType><![CDATA[event]]></MsgType>
    <Event><![CDATA[CLICK]]></Event>
    <EventKey><![CDATA[EVENTKEY]]></EventKey>
    </xml>
     */
    public class NormalMenuEventMessage : BaseMessage
    {

        /// <summary>
        /// 事件KEY值，设置的跳转URL
        /// </summary>
        public string EventKey { get; set; }
    }

    /// <summary>
    /// 菜单扫描事件
    /// </summary>
    /*
     <xml><ToUserName><![CDATA[ToUserName]]></ToUserName>
    <FromUserName><![CDATA[FromUserName]]></FromUserName>
    <CreateTime>1419265698</CreateTime>
    <MsgType><![CDATA[event]]></MsgType>
    <Event><![CDATA[scancode_push]]></Event>
    <EventKey><![CDATA[EventKey]]></EventKey>
    <ScanCodeInfo><ScanType><![CDATA[qrcode]]></ScanType>
    <ScanResult><![CDATA[http://weixin.qq.com/r/JEy5oRLE0U_urVbC9xk2]]></ScanResult>
    </ScanCodeInfo>
    </xml>
     */
    public class ScanMenuEventMessage : BaseMessage
    {

        /// <summary>
        /// 事件KEY值
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 扫码类型。qrcode是二维码，其他的是条码
        /// </summary>
        public string ScanType { get; set; }
        /// <summary>
        /// 扫描结果
        /// </summary>
        public string ScanResult { get; set; }
    }

}