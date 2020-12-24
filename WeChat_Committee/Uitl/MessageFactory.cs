using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using WeChat_Committee.Model;

namespace WeChat_Committee.Uitl
{
    /// <summary>
    /// 将xml数据包转换成对象
    /// </summary>
    public class MessageFactory
    {
        private static List<MsgFlag> _queue;
        public static BaseMessage CreateMessage(string xml)
        {
            if (_queue == null)
            {
                _queue = new List<MsgFlag>();
            }
            else if (_queue.Count >= 50)
            {
                _queue = _queue.Where(q => { return q.CreateTime.AddSeconds(20) > DateTime.Now; }).ToList();//保留20秒内未响应的消息
            }
            XElement xdoc = XElement.Parse(xml);
            var msgtype = xdoc.Element("MsgType").Value.ToUpper();
            var FromUserName = xdoc.Element("FromUserName").Value;
            var MsgId = xdoc.Element("MsgId").Value;
            var CreateTime = xdoc.Element("CreateTime").Value;
            MsgType type = (MsgType)Enum.Parse(typeof(MsgType), msgtype);
            if (type != MsgType.EVENT)
            {
                if (_queue.FirstOrDefault(m => { return m.Flag == MsgId; }) == null)
                {
                    _queue.Add(new MsgFlag
                    {
                        CreateTime = DateTime.Now,
                        FromUser = FromUserName,
                        Flag = MsgId
                    });
                }
                else
                {
                    return null;
                }

            }
            else
            {
                if (_queue.FirstOrDefault(m => { return m.Flag == CreateTime; }) == null)
                {
                    _queue.Add(new MsgFlag
                    {
                        CreateTime = DateTime.Now,
                        FromUser = FromUserName,
                        Flag = CreateTime
                    });
                }
                else
                {
                    return null;
                }
            }
            switch (type)
            {
                case MsgType.TEXT: return Common.XMLConvertObj<TextMessage>(xml);
                case MsgType.IMAGE: return Common.XMLConvertObj<ImgMessage>(xml);
                case MsgType.VIDEO: return Common.XMLConvertObj<VideoMessage>(xml);
                case MsgType.VOICE: return Common.XMLConvertObj<VoiceMessage>(xml);
                case MsgType.LINK:return Common.XMLConvertObj<LinkMessage>(xml);
                case MsgType.LOCATION:return Common.XMLConvertObj<LocationEventMessage>(xml);
                case MsgType.EVENT://事件类型
                    {
                        var eventtype = (Event)Enum.Parse(typeof(Event), xdoc.Element("Event").Value.ToUpper());
                        switch (eventtype)
                        {
                            case Event.CLICK:return Common.XMLConvertObj<NormalMenuEventMessage>(xml);
                            case Event.VIEW: return Common.XMLConvertObj<NormalMenuEventMessage>(xml);
                            case Event.LOCATION: return Common.XMLConvertObj<LocationEventMessage>(xml);
                            //case Event.LOCATION_SELECT: return Common.XMLConvertObj<LocationMenuEventMessage>(xml);
                            case Event.SCAN: return Common.XMLConvertObj<ScanEventMessage>(xml);
                            case Event.SUBSCRIBE: return Common.XMLConvertObj<SubEventMessage>(xml);
                            case Event.UNSUBSCRIBE: return Common.XMLConvertObj<SubEventMessage>(xml);
                            case Event.SCANCODE_WAITMSG: return Common.XMLConvertObj<ScanMenuEventMessage>(xml);
                            default:return Common.XMLConvertObj<BaseMessage>(xml);
                        }
                    }
                default:
                    return Common.XMLConvertObj<BaseMessage>(xml);
            }
        }

    }
}