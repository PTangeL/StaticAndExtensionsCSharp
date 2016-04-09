using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace StaticAndExtensionsCSharp.Images
{
    public class ImageUtils
    {
        public static bool ConvertTo(Image img, string saveToPath, ImageFormat format = default(ImageFormat))
        {

            if (File.Exists(saveToPath))
            {
                File.Delete(saveToPath);
            }

            img.Save(saveToPath, format);
            img.Dispose();

            return true;

        }
    }
}