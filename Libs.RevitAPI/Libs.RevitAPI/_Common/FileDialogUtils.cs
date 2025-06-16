using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace Libs.RevitAPI._Common
{
    public class FileDialogUtils
    {
        /// <summary>
        /// Sample
        /// Filter = "Excel (*.xlsx)|*.xlsx",
        /// FileName = "UniFormatChecker_" + DateTime.Now.ToString("yyyyMMdd")
        /// </summary>
        public static string SaveFile(string filter, string initialName)
        {
            string selectedPath = null;

            SaveFileDialog dialog = new SaveFileDialog
            {
                RestoreDirectory = true,
                Filter = filter,
                FileName = initialName
            };

            bool? rs = dialog.ShowDialog();
            if (rs.HasValue) if (rs.Value) selectedPath = dialog.FileName;

            return selectedPath;
        }

        public static string OpenFile(string filters)
        {
            string selectedPath = null;

            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = filters,
                CheckFileExists = true,
                RestoreDirectory = true
            };

            bool? rs = dialog.ShowDialog();
            if (rs.HasValue) if (rs.Value) selectedPath = dialog.FileName;

            return selectedPath;
        }

        public static List<string> OpenFiles(string filters)
        {
            List<string> paths = new List<string>();

            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = filters,
                CheckFileExists = true,
                RestoreDirectory = true,
                Multiselect = true
            };

            bool? rs = dialog.ShowDialog();
            if (rs.HasValue) if (rs.Value) paths = dialog.FileNames.ToList();

            return paths;
        }


        public static string SaveJsonFileDialog()
        {
            Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog
            {
                CheckPathExists = true,
                Filter = "Data Setting(*.json)|*.json",
                DefaultExt = ".json",
                RestoreDirectory = true
            };

            bool? result = saveFile.ShowDialog();
            if (result == true)
            {
                return saveFile.FileName;
            }
            return null;
        }
        public static string OpenJsonFileDialog()
        {
            //CreateGrids the dialog
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                //Set the dialog title
                Title = "Browse for Data Setting",
                //Perform checks
                CheckFileExists = true,
                CheckPathExists = true,
                //Set the file type filter
                Filter = "Data Setting(*.json)|*.json",
                // Set default extension
                DefaultExt = "json",

                //Open to last directory
                RestoreDirectory = true,
                //Include readOnly files
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
            //If the user clicks ok show the path in the textbox
            if (openFileDialog1.ShowDialog() == true)
            {
                return openFileDialog1.FileName;
            }

            return null;
        }
    }
}