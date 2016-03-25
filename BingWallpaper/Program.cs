using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BingWallpaper
{
    static class Program
    {
        static void Main()
        {
            string requestUri = @"http://www.bing.com/HPImageArchive.aspx?format=js&mbl=1&idx=0&n=1&video=1";
            string picFilePath = "./Wallpaper/";
            string startDate = string.Empty;
            string picFileName = string.Empty;
            var dicts = new Dictionary<string, object>();

            try
            {
                var request = WebRequest.Create(requestUri);
                var response = request.GetResponse();
                var respStream = response.GetResponseStream();
                var streamReader = new StreamReader(respStream, Encoding.GetEncoding("utf-8"));
                dicts = JsonConvert.DeserializeObject<Dictionary<string, object>>(streamReader.ReadLine());
            }
            catch (Exception)
            {
                ErrorMessage("背景图信息获取错误！");
                return;
            }

            try
            {
                var images = (JArray)dicts["images"];
                startDate = images[0]["startdate"].ToString();
                picFileName = picFilePath + startDate + ".jpg";
                requestUri = images[0]["url"].ToString();
            }
            catch (Exception)
            {
                ErrorMessage("背景图信息解析错误！");
                return;
            }

            if (!Directory.Exists(picFilePath))
            {
                try
                {
                    Directory.CreateDirectory(picFilePath);
                }
                catch (Exception)
                {
                    ErrorMessage("建立背景图下载目录错误！");
                    return;
                }
            }

            try
            {
                if (File.Exists(picFileName))
                {
                    ErrorMessage("背景图已存在，无需下载！");
                    return;
                }
                else
                {
                    var client = new WebClient();
                    client.DownloadFile(requestUri, picFileName);
                }
            }
            catch (Exception)
            {
                ErrorMessage("背景图保存错误！");
                return;
            }
            MessageBox.Show("背景图 <" + startDate + "> 下载完毕!");
        }

        private static void ErrorMessage(string message)
        {
            MessageBox.Show(message, "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
