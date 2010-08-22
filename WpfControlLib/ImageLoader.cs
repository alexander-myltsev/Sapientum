using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace WpfControlLib
{
    public static class ImageLoader
    {
        public static List<BitmapImage> LoadImages()
        {
            var directoryInfo = new DirectoryInfo(@"D:\Pictures");
            return directoryInfo.GetFiles("*.jpg")
                .Select(bitmapImage => new Uri(bitmapImage.FullName))
                .Select(uri => new BitmapImage(uri)).ToList();
        }
    }
}
