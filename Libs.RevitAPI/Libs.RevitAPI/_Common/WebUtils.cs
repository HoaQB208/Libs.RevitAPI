using System.IO;
using System.Net;
using System.Text;

namespace Libs.RevitAPI._Common
{
    public class WebUtils
    {
        public static string GetStringFromUrl(string url)
        {
            string data = "";
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                Stream stream = request.GetResponse().GetResponseStream();
                StreamReader strReader = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
                data = strReader.ReadLine();
            }
            catch { }
            return data;
        }
    }
}