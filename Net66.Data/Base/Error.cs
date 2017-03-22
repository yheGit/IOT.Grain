using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Data.Base
{
    public class Error
    {
        public static void WriteLog(string fileName, string msg)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }
            string text = AppDomain.CurrentDomain.BaseDirectory;
            text = text + "\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd");
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "error" + DateTime.Now.Year.ToString().Trim() + DateTime.Now.Month.ToString().Trim() + DateTime.Now.Day.ToString().Trim();
            }
            fileName += ".txt";
            string path = text + "\\" + fileName;
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(path, true))
                {
                    streamWriter.WriteLine("出错时间：" + DateTime.Now.ToString());
                    streamWriter.Write(msg);
                    streamWriter.WriteLine();
                    streamWriter.WriteLine("*****************************************************");
                    streamWriter.Close();
                }
            }
            catch
            {
            }
        }
    }
}
