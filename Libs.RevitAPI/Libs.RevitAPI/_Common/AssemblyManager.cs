using System.IO;
using System.Reflection;

namespace Libs.RevitAPI._Common
{
    public class AssemblyManager
    {
        public static void Load(string assemblyName)
        {
            string dllPath = Assembly.GetExecutingAssembly().Location;
            string folder = Path.GetDirectoryName(dllPath);
            var assemblyPath = Path.Combine(folder, assemblyName);
            if (File.Exists(assemblyPath))
            {
                AssemblyName assembly = AssemblyName.GetAssemblyName(assemblyPath);
                Assembly.Load(assembly);
            }
        }

        public static void LoadFulPath(string assemblyFullPath)
        {
            AssemblyName assembly = AssemblyName.GetAssemblyName(assemblyFullPath);
            Assembly.Load(assembly);
        }
    }
}