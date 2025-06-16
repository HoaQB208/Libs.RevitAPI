using System;
using System.Text.RegularExpressions;

namespace Libs.RevitAPI._Common
{
    public class StringUtils
    {
        public static string WildCardToRegular(string value)
        {
            return "^" + Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*") + "$";
        }

        /// <summary>
        /// return a ProperCase string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ProperCase(string text)
        {
            string kq = "";
            string str = text.ToLower();
            string[] dsStr = str.Split(new[] { " " }, StringSplitOptions.None);
            foreach (string item in dsStr)
            {
                if (item == "")
                {
                    kq += " ";
                }
                else
                {
                    kq += " " + item.Substring(0, 1).ToUpper() + item.Substring(1);
                }
            }
            return kq.Trim();
        }

        /// <summary>
        /// return a SentenceCase string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SentenceCase(string text)
        {
            string str = text.ToLower();
            string str1 = str.Substring(0, 1).ToUpper();
            string str2 = str.Substring(1);
            return (str1 + str2).Trim();

        }
    }
}
