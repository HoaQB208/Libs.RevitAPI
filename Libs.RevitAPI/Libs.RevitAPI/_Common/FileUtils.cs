using System;
using System.IO;

namespace Libs.RevitAPI._Common
{
    public class FileUtils
    {
        /// <summary>
        /// Đọc nội dung file text (ví dụ .txt, .json...). Truyền vào đường dẫn file, trả về nội dung file dạng string
        /// </summary>
        public static string ReadFile(string pathFile)
        {
            string st = "";
            if (File.Exists(pathFile))
            {
                try
                {
                    StreamReader sr = new StreamReader(pathFile);
                    st = sr.ReadToEnd();
                    sr.Close();
                }
                catch { }
            }
            return st;
        }

        /// <summary>
        /// Ghi string ra file
        /// </summary>
        public static string WriteFile(string pathFile, string content)
        {
            try
            {
                string pathfolder = Path.GetDirectoryName(pathFile);
                if (!Directory.Exists(pathfolder)) Directory.CreateDirectory(pathfolder);
                StreamWriter sw = new StreamWriter(pathFile);
                sw.Write(content);
                sw.Close();
            }
            catch (Exception ex) { return ex.ToString(); }
            return "";
        }


        /// <summary>
        /// Trả về đường dẫn của file dll chứa đoạn code này. Nhấn mạnh, 'của file dll CHỨA đoạn code này'
        /// </summary>
        public static string GetAssemblyPath()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().Location;
        }
    }
}