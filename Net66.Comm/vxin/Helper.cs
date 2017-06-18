using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Net66.Comm.vxin
{
    public class Helper
    {

        /// <summary>
        /// 发送人
        /// </summary>
        public const string FROM_USERNAME = "FromUserName";
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public const string TO_USERNAME = "ToUserName";
        /// <summary>
        /// 消息内容
        /// </summary>
        public const string CONTENT = "Content";
        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public const string CREATE_TIME = "CreateTime";
        /// <summary>
        /// 消息类型
        /// </summary>
        public const string MSG_TYPE = "MsgType";
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public const string MSG_ID = "MsgId";

        /// <summary>
        /// 微信token
        /// </summary>
        public static string WeiXinToken { get { return ConfigurationManager.AppSettings["WeiXinToken"].ToString(); } }

        /// <summary>
        /// 微信appID
        /// </summary>
        public static string AppID{ get { return ConfigurationManager.AppSettings["AppID"].ToString(); }}


        /// <summary>
        /// 微信Aappsecret
        /// </summary>
        public static string Appsecret { get { return ConfigurationManager.AppSettings["Appsecret"].ToString(); } }


        /// <summary>
        /// 创建二维码地址
        /// </summary>
        public static string CreateQrcode_URL { get { return "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token="; } }

        /// <summary>
        /// 换取二维码地址
        /// </summary>
        public static string GetQrcode_URL { get { return "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket="; } }

        /// <summary>
        /// 获得access_token地址
        /// </summary>
        public static string Access_token_URL { get { return string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", AppID, Appsecret); } }

        /// <summary>
        /// 通过code换取网页授权access_token地址
        /// </summary>
        public static string Web_Access_token_URL { get { return string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&grant_type=authorization_code&code=", AppID, Appsecret); } }
        
        
        /// <summary>
        /// 微信菜单创建提交地址
        /// </summary>
        public static string MENU_POST_URL { get { return "https://api.weixin.qq.com/cgi-bin/menu/create?access_token="; } }


        /// <summary>
        /// 微信向用户发送消息
        /// </summary>
        public static string SendMsg_Url { get { return "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token="; } }
        //https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=XHuQTIJr7NKmOvFQ4O2QXst_scnxIgqqhPy0rfbyBGgbzXt86x8NomCMLoFkCg9Yrnf_bsDS3WDB7D7fczwY93pZzHUwALPKpXu_tz0RGCRlam7WJcHY-E_0z0AwhXtiSVRgAHAADZ


        /// <summary>
        /// 微信获取用户信息
        /// </summary>
        public static string GetUserInfo_Url { get { return "https://api.weixin.qq.com/cgi-bin/user/info/batchget?access_token="; } }



    }
}
