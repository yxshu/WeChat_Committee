using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat_Committee.Model
{
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
            Utils.ResponseWrite("");
        }
        public virtual void ResText(EnterParam param, string content)
        {

        }
        /// <summary>
        /// 回复消息(音乐)
        /// </summary>
        public void ResMusic(EnterParam param, Music mu)
        {
        }
        public void ResVideo(EnterParam param, Video v)
        {
        }

        /// <summary>
        /// 回复消息(图片)
        /// </summary>
        public void ResPicture(EnterParam param, Picture pic, string domain)
        { }

        /// <summary>
        /// 回复消息（图文列表）
        /// </summary>
        /// <param name="param"></param>
        /// <param name="art"></param>
        public void ResArticles(EnterParam param, List<Articles> art)
        {
        }
        /// <summary>
        /// 多客服转发
        /// </summary>
        /// <param name="param"></param>
        public void ResDKF(EnterParam param)
        {
        }
        /// <summary>
        /// 多客服转发如果指定的客服没有接入能力(不在线、没有开启自动接入或者自动接入已满)，该用户会一直等待指定客服有接入能力后才会被接入，而不会被其他客服接待。建议在指定客服时，先查询客服的接入能力指定到有能力接入的客服，保证客户能够及时得到服务。
        /// </summary>
        /// <param name="param">用户发送的消息体</param>
        /// <param name="KfAccount">多客服账号</param>
        public void ResDKF(EnterParam param, string KfAccount)
        {
        }
        private void Response(EnterParam param, string data)
        {

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