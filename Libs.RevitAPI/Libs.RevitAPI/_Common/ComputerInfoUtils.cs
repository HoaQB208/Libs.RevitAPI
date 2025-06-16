using System.Collections.Generic;
using System.Management;

namespace Libs.RevitAPI._Common
{
    public class ComputerInfoUtils
    {
        public static string GetComputerInfo()
        {
            string computerName = System.Environment.MachineName;
            string userName = System.Environment.UserName;
            string diskSeris = "{" + GetDiskDriveSerials() + "}";
            return $"Computer={computerName}, UserName={userName}, DiskSeris={diskSeris}";
        }

        public static string GetDiskDriveSerials()
        {
            List<string> serials = new List<string>();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (ManagementBaseObject obj in searcher.Get())
                {
                    try
                    {
                        string seri = obj["SerialNumber"].ToString().Trim();
                        if (string.IsNullOrEmpty(seri)) serials.Add(seri);
                    }
                    catch { }
                }
            }
            catch { }
            return string.Join("|", serials);
        }
    }
}