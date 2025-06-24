using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Libs.RevitAPI._Common
{
    public class ImageUtils
    {
        public static BitmapSource CreateBitmapSourceFromIco(Icon ico)
        {
            Bitmap bitmap = ico.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();
            return Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public static BitmapImage GetIcon(string iconName, string assemblyName)
        {
            Uri uri = new Uri("pack://application:,,,/" + assemblyName + ";component/Resources/" + iconName, UriKind.RelativeOrAbsolute);
            return new BitmapImage(uri);
        }

        public static BitmapImage ByteArrayToBitmapImage(byte[] imageData, int height, int width)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            using (var ms = new MemoryStream(imageData))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.DecodePixelHeight = height;
                bitmap.DecodePixelWidth = width;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }
    }
}