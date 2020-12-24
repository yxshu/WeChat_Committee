using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat_Committee.Model
{
    /// <summary>
    /// 消息的去重标识
    /// 微信服务器在5秒内收不到响应会断掉连接，并且重新发起请求，总共重试三次。
    /// 消息的去重普通消息和事件消息是有区别的。普通消息使用msgid,而事件消息使用FromUserName + CreateTime。
    /// 创建个静态列表_queue，用来存储消息列表，列表的类型是List<MsgFlag>.
    /// 在处理微信消息体前，首先判断列表是否实例化，如果没有实例化则实例化，否则判断列表的长度是否大于或等于50（这个可以自定义，用处就是微信并发的消息量），
    /// 如果大于或等于50，则保留20秒内未响应的消息（5秒重试一次，总共重试3次，就是15秒，保险起见这里写20秒）。
    ///获取当前消息体的消息类型，并根据_queue判断当前消息是否已经请求了。如果是事件则保存FromUser和创建时间。如果是普通消息则保存MsgFlag。
    /// </summary>
    public class MsgFlag
    {
        /// <summary>
        /// 发送者标识
        /// </summary>
        public string FromUser { get; set; }
        /// <summary>
        /// 消息表示。普通消息时，为msgid，事件消息时，为事件的创建时间
        /// </summary>
        public string Flag { get; set; }
        /// <summary>
        /// 添加到队列的时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /*
         if (_queue == null)
            {
                _queue = new List<BaseMsg>();
            }
            else if(_queue.Count>=50)
            {
                _queue = _queue.Where(q => { return q.CreateTime.AddSeconds(20) > DateTime.Now; }).ToList();//保留20秒内未响应的消息
            }
            XElement xdoc = XElement.Parse(xml);
            var msgtype = xdoc.Element("MsgType").Value.ToUpper();
            var FromUserName = xdoc.Element("FromUserName").Value;
            var MsgId = xdoc.Element("MsgId").Value;
            var CreateTime = xdoc.Element("CreateTime").Value;
            MsgType type = (MsgType)Enum.Parse(typeof(MsgType), msgtype);
            if (type!=MsgType.EVENT)
            {
                if (_queue.FirstOrDefault(m => { return m.MsgFlag == MsgId; }) == null)
                {
                    _queue.Add(new BaseMsg
                    {
                        CreateTime = DateTime.Now,
                        FromUser = FromUserName,
                        MsgFlag = MsgId
                    });
                }
                else
                {
                    return null;
                }
               
            }
            else
            {
                if (_queue.FirstOrDefault(m => { return m.MsgFlag == CreateTime; }) == null)
                {
                    _queue.Add(new BaseMsg
                    {
                        CreateTime = DateTime.Now,
                        FromUser = FromUserName,
                        MsgFlag = CreateTime
                    });
                }
                else
                {
                    return null;
                }
            }
         
         */
    }
}