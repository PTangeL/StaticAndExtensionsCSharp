using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace Library.Images
{
    public class ImageUtils
    {
        private static Image.GetThumbnailImageAbort abortDelegate = new Image.GetThumbnailImageAbort(delegate { return false; });

        private static Size ExpandToBound(Size imageSize, Size boundingBox)
        {
            double widthScale = 0, heightScale = 0;
            if (imageSize.Width != 0)
            {
                widthScale = (double)boundingBox.Width / (double)imageSize.Width;
            }
            if (imageSize.Height != 0)
            {
                heightScale = (double)boundingBox.Height / (double)imageSize.Height;
            }
            double scale = Math.Min(widthScale, heightScale);
            Size result = new Size(
                (int)(imageSize.Width * scale),
                (int)(imageSize.Height * scale));
            return result;
        }

        public static bool ConvertToJpg(Image img, string saveToPath)
        {

            if (File.Exists(saveToPath))
            {
                File.Delete(saveToPath);
            }

            img.Save(saveToPath, ImageFormat.Jpeg);
            img.Dispose();

            return true;

        }

        public static Image ScaleImage(Image sourceImage, int maxWidth, int maxHeight)
        {
            Size newSize = ExpandToBound(sourceImage.Size, new Size(maxWidth, maxHeight));
            return sourceImage.GetThumbnailImage(newSize.Width, newSize.Height, abortDelegate, IntPtr.Zero);
        }
    }
}