using KimiReader.Platform.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KimiReader.Comm;

namespace KimiReader.Web.Controls.Msg
{
    public class VoiceText
    {

        public static string GetVoiceText(MultimediaMessage mm)
        {
            var msg = string.Format(MsgUtils.IotBackCommMsg, mm.FromUserName, mm.ToUserName, Utils.GetNowTime(), "voice", mm.MediaId, mm.Format, mm.MsgId);

            return msg;
        }

    }
}