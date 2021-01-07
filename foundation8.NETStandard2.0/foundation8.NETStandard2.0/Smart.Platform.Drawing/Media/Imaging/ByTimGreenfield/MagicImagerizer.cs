using System.Drawing;
using System.IO;

namespace Smart.Platform.Drawing.Media.Imaging.ByTimGreenfield
{
    public class MagicImagerizer
    {
        public static Bitmap GetImageSource(Stream stream, double maxWidth, double maxHeight)
        {
            Bitmap r = new Bitmap(stream);

            Image i = ResizeImage(r, new RectangleF(0.0f, 0.0f, (float)maxWidth, (float)maxHeight));
            r = new Bitmap(i);

            return r;
        }

        public static Bitmap GetImageSource(Bitmap bi, double maxWidth, double maxHeight)
        {           
            Image i = ResizeImage(bi, new RectangleF(0.0f, 0.0f, (float)maxWidth, (float)maxHeight));
            Bitmap r = new Bitmap(i);

            return r;
        }

        public static Image ResizeImage(Image source, RectangleF destinationBounds)
        {
            RectangleF sourceBounds = new RectangleF(0.0f, 0.0f, (float)source.Width, (float)source.Height);
            //RectangleF scaleBounds = new RectangleF();

            Image destinationImage = new Bitmap((int)destinationBounds.Width, (int)destinationBounds.Height);
            Graphics graph = Graphics.FromImage(destinationImage);
            graph.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            // Fill with background color
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), destinationBounds);

            float resizeRatio, sourceRatio;
            float scaleWidth, scaleHeight;

            sourceRatio = (float)source.Width / (float)source.Height;

            if (sourceRatio >= 1.0f)
            {
                //landscape
                resizeRatio = destinationBounds.Width / sourceBounds.Width;
                scaleWidth = destinationBounds.Width;
                scaleHeight = sourceBounds.Height * resizeRatio;
                float trimValue = destinationBounds.Height - scaleHeight;
                graph.DrawImage(source, 0, (trimValue / 2), destinationBounds.Width, scaleHeight);
            }
            else
            {
                //portrait
                resizeRatio = destinationBounds.Height / sourceBounds.Height;
                scaleWidth = sourceBounds.Width * resizeRatio;
                scaleHeight = destinationBounds.Height;
                float trimValue = destinationBounds.Width - scaleWidth;
                graph.DrawImage(source, (trimValue / 2), 0, scaleWidth, destinationBounds.Height);
            }

            return destinationImage;

        }
    }
}
