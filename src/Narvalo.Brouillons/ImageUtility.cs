// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;

    public static class ImageUtility
    {
        private static ImageCodecInfo s_AvailableJpegEncoder;

        public static ImageCodecInfo JpegEncoder
        {
            get
            {
                if (s_AvailableJpegEncoder == null)
                {
                    ImageCodecInfo[] ici = ImageCodecInfo.GetImageDecoders();

                    foreach (ImageCodecInfo info in ici)
                    {
                        if (info.FormatID == ImageFormat.Jpeg.Guid)
                        {
                            s_AvailableJpegEncoder = info;
                            break;
                        }
                    }
                }

                return s_AvailableJpegEncoder;
            }
        }

        public static void ResizeImage(string inFile, string outFile, double maxWidth, double maxHeight, long level)
        {
            Require.NotNullOrEmpty(inFile, "inFile");
            Require.NotNullOrEmpty(outFile, "outFile");
            Require.GreaterThanOrEqualTo(level, 0, "level");
            Require.LessThanOrEqualTo(level, 100, "level");

            using (var outStream = new FileStream(outFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                // Load via stream rather than Image.FromFile to release the file handle immediately.
                using (var stream = new FileStream(inFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var inImage = Image.FromStream(stream))
                    {
                        ResizeImage(inImage, outStream, maxWidth, maxHeight, level);
                    }
                }
            }
        }

        public static void ResizeImage(Image inImage, string outFile, double maxWidth, double maxHeight, long level)
        {
            Require.NotNull(inImage, "inImage");
            Require.NotNullOrEmpty(outFile, "outFile");
            Require.GreaterThanOrEqualTo(level, 0, "level");
            Require.LessThanOrEqualTo(level, 100, "level");

            using (var outStream = new FileStream(outFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                ResizeImage(inImage, outStream, maxWidth, maxHeight, level);
            }
        }

        public static void ResizeImage(Image inImage, Stream outStream, double maxWidth, double maxHeight, long level)
        {
            Require.NotNull(inImage, "inImage");
            Require.NotNull(outStream, "outStream");
            Require.GreaterThanOrEqualTo(level, 0, "level");
            Require.LessThanOrEqualTo(level, 100, "level");

            var dim = GetFitToHeightDimensions(inImage.Width, inImage.Height, maxWidth, maxHeight);

            using (var bitmap = Resize(inImage, dim.Width, dim.Height))
            {
                //if (inImage.RawFormat.Guid == ImageFormat.Jpeg.Guid) {
                if (JpegEncoder == null)
                {
                    bitmap.Save(outStream, inImage.RawFormat);
                }
                else
                {
                    SaveAsJpeg(bitmap, outStream, level);
                }
                //}
                //else {
                //    // Fill with white for transparent GIFs
                //    //graphics.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
                //    bitmap.Save(outStream, inImage.RawFormat);
                //}
            }
        }

        struct Dimensions
        {
            private readonly int _width;
            private readonly int _height;

            public Dimensions(int width, int height)
            {
                _width = width;
                _height = height;
            }

            public int Width { get { return _width; } }
            public int Height { get { return _height; } }
        }

        private static Dimensions GetFitToHeightDimensions(int width, int height, double maxWidth, double maxHeight)
        {
            //double width;
            //double height;

            //if (inImage.Height < inImage.Width) {
            //    width = maxWidth;
            //    height = (maxHeight / (double)inImage.Width) * inImage.Height;
            //}
            //else {
            //    width = (maxWidth / (double)inImage.Height) * inImage.Width;
            //    height = maxHeight;
            //}

            double aspectRatio = width / height;
            double boxRatio = maxWidth / maxHeight;
            double scaleFactor;

            if (boxRatio > aspectRatio)
            {
                // Use height, since that is the most restrictive dimension of box.
                scaleFactor = maxHeight / height;
            }
            else
            {
                scaleFactor = maxWidth / width;
            }

            return new Dimensions((int)(width * scaleFactor), (int)(height * scaleFactor));
        }

        private static Bitmap Resize(Image image, int width, int height)
        {
            Bitmap bitmap;
            Bitmap tmpBitmap = null;

            try
            {
                tmpBitmap = new Bitmap(width, height);

                using (var graphics = Graphics.FromImage(tmpBitmap))
                {
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.DrawImage(image, 0, 0, tmpBitmap.Width, tmpBitmap.Height);
                }

                bitmap = tmpBitmap;
                tmpBitmap = null;
            }
            finally
            {
                if (tmpBitmap != null)
                {
                    tmpBitmap.Dispose();
                }
            }

            return bitmap;
        }

        private static void SaveAsJpeg(Bitmap bitmap, Stream outStream, long level)
        {
            using (var ep = new EncoderParameters(1))
            {
                ep.Param[0] = new EncoderParameter(Encoder.Quality, level);
                bitmap.Save(outStream, s_AvailableJpegEncoder, ep);
            }
        }
    }
}
