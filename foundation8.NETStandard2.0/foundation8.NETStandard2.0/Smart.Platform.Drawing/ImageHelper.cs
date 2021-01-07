using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Smart.Platform.Diagnostics;

namespace Smart.Platform.Drawing
{
    public enum ScaleModes
    {
        Percent,
        Pad,
        Crop
    }

    internal class ScaleImageParameters
    {
        #region Constructors

        private ScaleImageParameters() { }

        internal ScaleImageParameters(Image image, ScaleModes scaleMode)
        {
            _image      = image;
            _scaleMode  = scaleMode;
        }

        internal ScaleImageParameters(Image image, int requestedPercent)
        {
            _image              = image;
            _scaleMode          = ScaleModes.Percent;
            _requestedPercent   = requestedPercent;
        }

        internal ScaleImageParameters(Image image, ScaleModes scaleMode, int requestedWidth, int requestedHeight) 
        {
            _image              = image;
            _scaleMode          = scaleMode;
            _requestedWidth     = requestedWidth;
            _requestedHeight    = requestedHeight;
        }

        #endregion

        #region Internal Methods

        private Image _image;

        internal Image Image
        {
            get { return _image; }
            set { _image = value; }
        }

        private ScaleModes _scaleMode;

        internal ScaleModes ScaleMode
        {
            get { return _scaleMode; }
            set { _scaleMode = value; }
        }

        private int _requestedPercent;

        internal int RequestedPercent
        {
            get { return _requestedPercent; }
            set { _requestedPercent = value; }
        }

        private int _requestedWidth;

        internal int RequestedWidth
        {
            get { return _requestedWidth; }
            set { _requestedWidth = value; }
        }

        private int _requestedHeight;

        internal int RequestedHeight
        {
            get { return _requestedHeight; }
            set { _requestedHeight = value; }
        }

        private int _x = 0;

        internal int X
        {   
            get { return _x; } 
            set { _x = value; } 
        }

        private int _y = 0;

        internal int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        private int _canvasWidth;

        internal int CanvasWidth
        {
            get { return _canvasWidth; }
            set { _canvasWidth = value; }
        }

        private int _canvasHeight;

        internal int CanvasHeight
        {
            get { return _canvasHeight; }
            set { _canvasHeight = value; }
        }

        private int _drawWidth;

        internal int DrawWidth
        {
            get { return _drawWidth; }
            set { _drawWidth = value; }
        }

        private int _drawHeight;

        internal int DrawHeight
        {
            get { return _drawHeight; }
            set { _drawHeight = value; }
        }

        private float _percentActual = 1;

        internal float PercentActual
        {
            get { return _percentActual; }
            set { _percentActual = value; }
        }

        private float _percentWidthRequired = 1;

        internal float PercentWidthRequired
        {
            get { return _percentWidthRequired; }
            set { _percentWidthRequired = value; }
        }

        private float _percentHeightRequired = 1;

        internal float PercentHeightRequired
        {
            get { return _percentHeightRequired; }
            set { _percentHeightRequired = value; }
        }

        private bool _fixedWidth = true;

        internal bool FixedWidth
        {
            get { return _fixedWidth; }
            set { _fixedWidth = value; }
        }

        private bool _fixedHeight = false;

        internal bool FixedHeight
        {
            get { return _fixedHeight; }
            set { _fixedHeight = value; }
        }

        #endregion
    }

    /// <summary>
    /// A helper class for images.
    /// </summary>
    public class ImageHelper
    {
        #region Constructors

        private ImageHelper() { }

        #endregion

        #region Public Static Methods

        public static Image ScaleToPercent(Image image, int percent)
        {
            #region Check Parameters

            if (image == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "image is nothing"));
            if (percent <= 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "percent is invalid"));

            #endregion

            float actualPercent = ((float)percent / 100);

            // The scaling parameters
            int canvasWidth     = (int)(image.Width * actualPercent);
            int canvasHeight    = (int)(image.Height * actualPercent);
            int x = 0;
            int y = 0;

            return RedrawImage(image, canvasWidth, canvasHeight, canvasWidth, canvasHeight, x, y, image.PixelFormat, 
                                image.HorizontalResolution, image.VerticalResolution, Color.Black, 
                                InterpolationMode.HighQualityBicubic);
        }

        public static Image ScaleToWidth(Image image, int width, ScaleModes scaleMode)
        {
            #region Check Parameters

            if (image == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "image is nothing"));
            if (width <= 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "width is invalid"));

            #endregion

            ScaleImageParameters p = new ScaleImageParameters(image, scaleMode);
            p.RequestedWidth    = width;
            p.FixedWidth        = true;
            p.FixedHeight       = false;

            SetScaleImageParameters(p);

            return RedrawImage(image, p.CanvasWidth, p.CanvasHeight, p.DrawWidth, p.DrawHeight,
                                p.X, p.Y, image.PixelFormat, image.HorizontalResolution,
                                image.VerticalResolution, Color.Black, InterpolationMode.HighQualityBicubic);
        }

        public static Image ScaleToHeight(Image image, int height, ScaleModes scaleMode)
        {
            #region Check Parameters

            if (image == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "image is nothing"));
            if (height <= 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "height is invalid"));

            #endregion

            ScaleImageParameters p = new ScaleImageParameters(image, scaleMode);
            p.RequestedHeight   = height;
            p.FixedWidth        = false;
            p.FixedHeight       = true;

            SetScaleImageParameters(p);

            return RedrawImage(image, p.CanvasWidth, p.CanvasHeight, p.DrawWidth, p.DrawHeight,
                                p.X, p.Y, image.PixelFormat, image.HorizontalResolution,
                                image.VerticalResolution, Color.Black, InterpolationMode.HighQualityBicubic);
        }

        public static Image ScaleToSize(Image image, int width, int height, ScaleModes scaleMode)
        {
            #region Check Parameters

            if (image == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "image is nothing"));
            if (width <= 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "width is invalid"));
            if (height <= 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "height is invalid"));

            #endregion

            ScaleImageParameters p = new ScaleImageParameters(image, scaleMode, width, height);
            p.FixedWidth    = true;
            p.FixedHeight   = true;

            SetScaleImageParameters(p);

            return RedrawImage( image, p.CanvasWidth, p.CanvasHeight, p.DrawWidth, p.DrawHeight, 
                                p.X, p.Y, image.PixelFormat, image.HorizontalResolution, 
                                image.VerticalResolution, Color.Black, InterpolationMode.HighQualityBicubic);
        }

        public static Image ScaleToMaxWidthHeight(Stream imageStream, int maxWidth, int maxHeight, ScaleModes scaleMode)
        {
            #region Check Parameters

            if (imageStream == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "imageStream is nothing"));
            if (maxWidth <= 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "maxWidth is invalid"));
            if (maxHeight <= 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "maxHeight is invalid"));

            #endregion

            Bitmap b = new Bitmap(imageStream);

            return ScaleToMaxWidthHeight(b, maxWidth, maxHeight, scaleMode);
        }

        public static Image ScaleToMaxWidthHeight(Image image, int maxWidth, int maxHeight, ScaleModes scaleMode)
        {
            #region Check Parameters

            if (image == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "image is nothing"));
            if (maxWidth <= 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "maxWidth is invalid"));
            if (maxHeight <= 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "maxHeight is invalid"));

            #endregion

            Image   r       = image;

            double  scaleX  = 1;
            double  scaleY  = 1;

            if (image.Height > maxHeight)   scaleY = (double)maxHeight / (double)image.Height;
            if (image.Width > maxWidth)     scaleX = (double)maxWidth / (double)image.Width;

            // Only perform the scale if it needs to be done
            if (scaleX < 1 || scaleY < 1)
            {
                // Maintain aspect ratio by picking the most severe scale
                double  scale       = Math.Min(scaleY, scaleX);

                int     newWidth    = maxHeight;
                int     newHeight   = maxWidth;

                // Height is greatest; set height to max, and scale width
                if (scale == scaleY && scaleY < scaleX)
                {
                    newWidth = Convert.ToInt32(image.Width * scale);
                }
                // Width is greatest; set width to max, and scale height
                else if (scale == scaleX && scaleX < scaleY)
                {
                    newHeight = Convert.ToInt32(image.Height * scale);
                }

                ScaleImageParameters p = new ScaleImageParameters(image, scaleMode, newWidth, newHeight);
                p.FixedWidth    = true;
                p.FixedHeight   = true;

                SetScaleImageParameters(p);

                r = RedrawImage(image, p.CanvasWidth, p.CanvasHeight, p.DrawWidth, p.DrawHeight, 
                    p.X, p.Y, image.PixelFormat, image.HorizontalResolution,
                    image.VerticalResolution, Color.Black, InterpolationMode.HighQualityBicubic);
            }

            return r;
        }

        public static Image RedrawImage(Image               image,
                                        int                 canvasWidth,
                                        int                 canvasHeight,
                                        int                 drawWidth,
                                        int                 drawHeight,
                                        int                 x,
                                        int                 y,
                                        PixelFormat         pixelFormat,
                                        float               horizontalResolution,
                                        float               verticalResolution,
                                        Color               backgroundColor,
                                        InterpolationMode   interpolationMode)
        {
            #region Check Parameters

            if (image == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "image is nothing"));
            if (canvasWidth <= 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "canvasWidth is invalid"));
            if (canvasHeight <= 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "canvasHeight is invalid"));
            if (drawWidth <= 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "drawWidth is invalid"));
            if (drawHeight <= 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "drawHeight is invalid"));
            if (horizontalResolution <= 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "horizontalResolution is invalid"));
            if (verticalResolution <= 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "verticalResolution is invalid"));

            #endregion

            // Setup a bitmap object to be edited
            Bitmap      imageBitmap     = new Bitmap(canvasWidth, canvasHeight, pixelFormat);
            imageBitmap.SetResolution(horizontalResolution, verticalResolution);

            // Note: To avoid getting a strange dark or fuzzy border, we have to use these image attributes
            ImageAttributes ia = new ImageAttributes();
            ia.SetWrapMode(WrapMode.TileFlipXY);

            // Setup a graphics object to edit the bitmap
            Graphics    imageGraphics   = Graphics.FromImage(imageBitmap);
            imageGraphics.Clear(backgroundColor);
            imageGraphics.InterpolationMode = interpolationMode;
            imageGraphics.PixelOffsetMode   = PixelOffsetMode.HighQuality;
            
            // Create parallelogram for drawing image
            Point ulCorner = new Point(x, y);
            Point urCorner = new Point(x + drawWidth, y);
            Point llCorner = new Point(x, y + drawHeight);
            Point[] destPara =  new Point[]{ulCorner, urCorner, llCorner};

            imageGraphics.DrawImage(    image, 
                                        destPara,                                        
                                        new Rectangle(0, 0, image.Width, image.Height),
                                        GraphicsUnit.Pixel, ia);
            imageGraphics.Dispose();

            return imageBitmap;
        }

        public static Image CopyImage(Image image)
        {
            #region Check Parameters

            if (image == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "image is nothing"));

            #endregion

            // Setup a bitmap object to be edited
            Bitmap imageBitmap = new Bitmap(image.Width, image.Height, image.PixelFormat);
            imageBitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            // Setup a graphics object to edit the bitmap
            Graphics imageGraphics = Graphics.FromImage(imageBitmap);
            imageGraphics.DrawImage(image,
                                    new Rectangle(0, 0, image.Width, image.Height),
                                    new Rectangle(0, 0, image.Width, image.Height),
                                    GraphicsUnit.Pixel);
            imageGraphics.Dispose();

            return imageBitmap;
        }

        /// <summary>
        /// Gets an image converted from the specified array of bytes.
        /// </summary>
        /// <param name="binary">The array of bytes representing the binary image.</param>
        /// <returns></returns>
        public static Image ByteArrayToImage(byte[] binary)
        {
            #region Check Parameters

            if (binary == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "binary is nothing"));

            #endregion

            MemoryStream ms = new MemoryStream(binary);
            try
            {            
                return Image.FromStream(ms);
            }
            finally
            {
                ms.Close();
            }
        }

        /// <summary>
        /// Gets an array of bytes converted from the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(Image image) 
        {
            #region Check Parameters

            if (image == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "image is nothing"));

            #endregion

            Image i = CopyImage(image);
            //image.Dispose();

            MemoryStream ms = new MemoryStream();
            try
            {
                i.Save(ms, image.RawFormat);

                return ms.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ms.Close();
                i.Dispose();
            }
        }

        public static Image Base64StringToImage(string binary)
        {
            #region Check Parameters

            if (binary == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "binary is nothing"));

            #endregion

            byte[]  b       = null;
            Image   result  = null;

            if (binary.Length > 0)
            {
                b       = Convert.FromBase64String(binary);
                result  = ImageHelper.ByteArrayToImage(b);
            }

            return result;
        }

        public static string ImageToBase64String(Image image)
        {
            #region Check Parameters

            if (image == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "image is nothing"));

            #endregion

            byte[] b        = ImageHelper.ImageToByteArray(image);
            string result   = Convert.ToBase64String(b);

            return result;
        }

        public static Stream EncodeToJpg(Stream imageStream)
        {
            #region Check Parameters

            if (imageStream == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "imageStream is nothing"));

            #endregion

            Bitmap b = new Bitmap(imageStream);

            Stream s = Smart.Platform.Drawing.Media.Imaging.ByTimGreenfield.JpgEncoder.Encode(b, 50);

            return s;
        }

        #endregion

        #region Private Methods

        private static void SetScaleImageParameters(ScaleImageParameters parameters)
        {
            #region Check Parameters

            if (parameters == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameters is nothing"));

            #endregion

            // Set the scaling percentages for width and height
            SetRequiredScalingPercentages(parameters);

            // Set the actual percentage to be used for scaling
            SetActualScalingPercentage(parameters);

            // Set the dimensions to be used for the scaling
            SetScalingDimensions(parameters);
        }

        private static void SetScalingDimensions(ScaleImageParameters parameters)
        {
            #region Check Parameters

            if (parameters == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameters is nothing"));

            #endregion

            // If the width is not being padded then set it to the requested width, otherwise
            // scale the width using the actual scaling percentage
            if (parameters.PercentActual == parameters.PercentWidthRequired && parameters.FixedWidth)
            {
                parameters.CanvasWidth = parameters.RequestedWidth;  
                parameters.DrawWidth   = parameters.CanvasWidth;          
            }
            else
            {
                if (parameters.FixedWidth)
                {
                    parameters.CanvasWidth = parameters.RequestedWidth;
                    parameters.DrawWidth   = (int)(parameters.Image.Width * parameters.PercentActual);
                }
                else
                {
                    parameters.CanvasWidth = (int)(parameters.Image.Width * parameters.PercentActual);
                    parameters.DrawWidth = parameters.CanvasWidth;
                }
            }

            // If the height is not being padded then set it to the requested height, otherwise
            // scale the height using the actual scaling percentage
            if (parameters.PercentActual == parameters.PercentHeightRequired && parameters.FixedHeight)
            {
                parameters.CanvasHeight = parameters.RequestedHeight;  
                parameters.DrawHeight   = parameters.CanvasHeight;          
            }
            else
            {
                if (parameters.FixedHeight)
                {
                    parameters.CanvasHeight = parameters.RequestedHeight;
                    parameters.DrawHeight   = (int)(parameters.Image.Height * parameters.PercentActual);
                }
                else
                {
                    parameters.CanvasHeight = (int)(parameters.Image.Height * parameters.PercentActual);
                    parameters.DrawHeight = parameters.CanvasHeight;
                }
            }
        }

        private static void SetActualScalingPercentage(ScaleImageParameters parameters)
        {
            #region Check Parameters

            if (parameters == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameters is nothing"));

            #endregion

            float[] requiredPercentages = { parameters.PercentWidthRequired, parameters.PercentHeightRequired };

            switch (parameters.ScaleMode)
            {
                case ScaleModes.Pad:
                    parameters.PercentActual = requiredPercentages.Min();
                    break;
                case ScaleModes.Crop:
                    parameters.PercentActual = requiredPercentages.Max();
                    break;
            }
        }

        private static void SetRequiredScalingPercentages(ScaleImageParameters parameters)
        {
            #region Check Parameters

            if (parameters == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameters is nothing"));

            #endregion

            if (parameters.FixedWidth)
            {
                parameters.PercentWidthRequired = ((float)parameters.RequestedWidth / (float)parameters.Image.Width);
                if (!parameters.FixedHeight) parameters.PercentHeightRequired = parameters.PercentWidthRequired;
            }
            if (parameters.FixedHeight)
            {
                parameters.PercentHeightRequired = ((float)parameters.RequestedHeight / (float)parameters.Image.Height);
                if (!parameters.FixedWidth) parameters.PercentWidthRequired = parameters.PercentHeightRequired;
            }
       }

        #endregion
    }
}
