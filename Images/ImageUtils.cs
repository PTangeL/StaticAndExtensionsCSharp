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

        /// <summary>
        /// Process an existing image, adding borders, title text and resizing.
        /// </summary>
        /// <param name="strSourcePath">Source path.</param>
        /// <param name="strImageSource">Source image filename.</param>
        /// <param name="strImageText">Title text to place on the image.</param>
        /// <param name="intNewWidth">Width of the processed image.</param>
        /// <param name="intNewHeight">Height of the processed image.</param>
        /// <param name="blnMakeThumb">Process this image as a thumbnail.</param>
        public static void ResizeImage(string strSourcePath, string strImageSource, string strImageText, int intNewWidth, int intNewHeight, bool blnMakeThumb)
        {
            ResizeImage(strSourcePath, strImageSource, strSourcePath, strImageSource, "#FFFFFF", strImageText, "#000000", intNewWidth, intNewHeight, blnMakeThumb, true, false, false);
        }
        /// <summary>
        /// Process an existing image, adding borders, title text and resizing.
        /// </summary>
        /// <param name="strSourcePath">Source path.</param>
        /// <param name="strImageSource">Source image filename.</param>
        /// <param name="strDestinationPath">Destination path.</param>
        /// <param name="strImageDestination">Destination image filename.</param>
        /// <param name="strBackgroundColor">Background color to use in HTML format (eg. #000000).</param>
        /// <param name="strImageText">Title text to place on the image.</param>
        /// <param name="strTextColor">Text color to use in HTML format (eg. #000000).</param>
        /// <param name="intNewWidth">Width of the processed image.</param>
        /// <param name="intNewHeight">Height of the processed image.</param>
        /// <param name="blnMakeThumb">Process this image as a thumbnail.</param>
        /// <param name="blnPreserveAspect">Preserve the original aspect ratio when resizing.</param>
        /// <param name="blnRoundEdges">Round edges on the image.</param>
        /// <param name="blnAllowEnlarge">Allow enlarging of the image. Set this parameter to
        /// false to limit the maximum filesize using the intNewWidth and intNewHeight parameters.</param>
        public static void ResizeImage(
            string strSourcePath,
            string strImageSource,
            string strDestinationPath,
            string strImageDestination,
            string strBackgroundColor,
            string strImageText,
            string strTextColor,
            int intNewWidth,
            int intNewHeight,
            bool blnMakeThumb,
            bool blnPreserveAspect,
            bool blnRoundEdges,
            bool blnAllowEnlarge)
        {
            int intCornerDiameter = (blnMakeThumb) ? 20 : 50;
            int intShift;

            float fltAspect, fltScale;
            Bitmap bmpSource, bmpDestination;
            Graphics g;
            Matrix transformMatrix;
            GraphicsPath gPath;
            InterpolationMode interpolationmode;
            Brush brush;
            ImageFormat imgFormat;
            Font font;
            bool blnQuantize = false;
            int intColors = 255, intColorBits = 8;
            
            bmpSource = new Bitmap(strSourcePath + strImageSource);
            imgFormat = bmpSource.RawFormat;

            if (blnMakeThumb)
            {
                strImageDestination = "th_" + strImageDestination;
            }

            if (intNewWidth == 0 || intNewHeight == 0)
            {
                intNewWidth = bmpSource.Width;
                intNewHeight = bmpSource.Height;
            }
            else
            {
                if (blnPreserveAspect)
                {
                    fltAspect = (float)bmpSource.Height / (float)bmpSource.Width;

                    if (bmpSource.Width > bmpSource.Height)
                    {
                        intNewHeight = (int)((float)intNewWidth * fltAspect);
                    }
                    else
                    {
                        intNewWidth = (int)((float)intNewHeight / fltAspect);
                    }
                }
            }

            fltScale = (float)(intNewWidth * intNewHeight) / (float)(bmpSource.Width * bmpSource.Height);

            interpolationmode = InterpolationMode.NearestNeighbor;

            if (fltScale < 1)
            {
                interpolationmode = InterpolationMode.HighQualityBilinear;
            }

            if (fltScale > 1)
            {
                if (blnAllowEnlarge)
                {
                    interpolationmode = InterpolationMode.HighQualityBicubic;
                }
                else
                {
                    intNewWidth = bmpSource.Width;
                    intNewHeight = bmpSource.Height;
                }
            }

            bmpDestination = new Bitmap(intNewWidth, intNewHeight, PixelFormat.Format32bppArgb);
            g = Graphics.FromImage(bmpDestination);
            g.InterpolationMode = interpolationmode;
            g.DrawImage(bmpSource, 0, 0, intNewWidth, intNewHeight);

            if (strImageText != null)
            {
                if (strImageText != string.Empty)
                {
                    if (!blnMakeThumb)
                    {
                        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                        brush = new SolidBrush(ColorTranslator.FromHtml(strTextColor));
                        font = new Font("Verdana", 8, FontStyle.Regular);

                        if (blnRoundEdges)
                        {
                            g.DrawString(strImageText, font, brush, Convert.ToInt32(intCornerDiameter / 6), 
                                Convert.ToInt32(intCornerDiameter / 6));
                        }
                        else
                        {
                            g.DrawString(strImageText, font, brush, 5, 5);
                        }
                        brush.Dispose();
                    }
                }
            }

            if (blnRoundEdges)
            {
                transformMatrix = g.Transform;

                g.SmoothingMode = SmoothingMode.HighQuality;

                brush = new SolidBrush(ColorTranslator.FromHtml(strBackgroundColor));
                gPath = new GraphicsPath();
                gPath.AddLine(-10, -10, 0, (intCornerDiameter / 2));
                gPath.AddArc(0, 0, intCornerDiameter, intCornerDiameter, 180, 90);
                gPath.AddLine((intCornerDiameter / 2), 0, -10, -10);
                gPath.CloseAllFigures();

                g.FillPath(brush, gPath);

                // Slight offset as we cannot align on half a pixel
                intShift = (blnMakeThumb) ? 1 : 0;

                g.TranslateTransform(intNewWidth - intShift, 0);
                g.RotateTransform(90);
                g.FillPath(brush, gPath);

                g.TranslateTransform(intNewHeight - intShift, 0);
                g.RotateTransform(90);
                g.FillPath(brush, gPath);

                g.TranslateTransform(intNewWidth - intShift, 0);
                g.RotateTransform(90);
                g.FillPath(brush, gPath);

                gPath.Dispose();
                brush.Dispose();

                g.Transform = transformMatrix;
            }

            if ((fltScale != 1 || (fltScale > 1 && !blnAllowEnlarge)) || blnRoundEdges || strImageText != null)
            {
                switch (bmpSource.PixelFormat)
                {
                    case PixelFormat.Indexed:
                    case PixelFormat.Format8bppIndexed:

                        blnQuantize = true;
                        intColors = 255;
                        intColorBits = 8;

                        break;

                    case PixelFormat.Format4bppIndexed:

                        blnQuantize = true;
                        intColors = 15;
                        intColorBits = 4;

                        break;

                    case PixelFormat.Format1bppIndexed:

                        blnQuantize = true;
                        intColors = 1;
                        intColorBits = 1;

                        break;
                }

                // See KB 814675
                g.Dispose();
                bmpSource.Dispose();

                if (blnQuantize)
                {
                    Quantizer cQuant = new Quantizer(intColors, intColorBits);

                    bmpDestination = cQuant.Quantize(bmpDestination);
                }

                bmpDestination.Save(strDestinationPath + strImageDestination, imgFormat);
            }
            else
            {
                if (!File.Exists(strDestinationPath + strImageDestination))
                {
                    File.Copy(strSourcePath + strImageSource, strDestinationPath + strImageDestination);
                }

                // See KB 814675
                g.Dispose();
                bmpSource.Dispose();
            }

            bmpDestination.Dispose();
        }
    }
}