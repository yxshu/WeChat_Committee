using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat_Committee.Model
{
    /// <summary>
    /// 上传到微信服务器媒体文件的类型定义为枚举
    /// 
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
    public enum MediaType
    {
        /// <summary>
        /// 图片（image）: 1M，支持JPG格式
        /// </summary>
        image,
        /// <summary>
        /// 语音（voice）：2M，播放长度不超过60s，支持AMR\MP3格式
        /// </summary>
        voice,
        /// <summary>
        /// 视频（video）：10MB，支持MP4格式
        /// </summary>
        video,
        /// <summary>
        /// 缩略图（thumb）：64KB，支持JPG格式
        /// </summary>
        thumb
    }
}
