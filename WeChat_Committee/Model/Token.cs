using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat_Committee.Model
{
    public class Token
    {
        private string access_token;
        private int expires_in;
        private DateTime createtime;
        /// <summary>
        /// 接口调用请求说明
        /// https请求方式: GET https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=APPSECRET
        /// 
        /// 返回码 	说明
        ///-1 	系统繁忙，此时请开发者稍候再试
        ///0 	请求成功
        ///40001 	AppSecret错误或者AppSecret不属于这个公众号，请开发者确认AppSecret的正确性
        ///40002 	请确保grant_type字段值为client_credential
        ///40164 	调用接口的IP地址不在白名单中，请在接口IP白名单中进行设置。（小程序及小游戏调用不要求IP地址在白名单内。）
        ///89503 	此IP调用需要管理员确认,请联系管理员
        ///89501 	此IP正在等待管理员确认,请联系管理员
        ///89506 	24小时内该IP被管理员拒绝调用两次，24小时内不可再使用该IP调用
        ///89507 	1小时内该IP被管理员拒绝调用一次，1小时内不可再使用该IP调用
        /// </summary>
        public Token()
        {
            this.Createtime = DateTime.Now;
        }

        /// <summary>
        /// 接口调用请求说明
        /// https请求方式: GET https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=APPSECRET
        /// 
        /// 返回码 	说明
        ///-1 	系统繁忙，此时请开发者稍候再试
        ///0 	请求成功
        ///40001 	AppSecret错误或者AppSecret不属于这个公众号，请开发者确认AppSecret的正确性
        ///40002 	请确保grant_type字段值为client_credential
        ///40164 	调用接口的IP地址不在白名单中，请在接口IP白名单中进行设置。（小程序及小游戏调用不要求IP地址在白名单内。）
        ///89503 	此IP调用需要管理员确认,请联系管理员
        ///89501 	此IP正在等待管理员确认,请联系管理员
        ///89506 	24小时内该IP被管理员拒绝调用两次，24小时内不可再使用该IP调用
        ///89507 	1小时内该IP被管理员拒绝调用一次，1小时内不可再使用该IP调用
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="expires_in"></param>
        /// <param name="createtime">tokon生成的日期+时间</param>
        public Token(string access_token, int expires_in)
        {
            this.Access_token = access_token;
            this.Expires_in = expires_in;
            this.Createtime = DateTime.Now;
        }
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string Access_token { get => access_token; set => access_token = value; }
        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int Expires_in { get => expires_in; set => expires_in = value; }
        public DateTime Createtime { get => createtime; set => createtime = value; }
    }
}