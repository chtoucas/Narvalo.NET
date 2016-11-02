// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    using System;
    using System.Collections.Generic;

    public sealed class ImageDevice : OutputDevice
    {
        public ImageDevice(ImageFormat format)
            : base()
        {
            ImageFormat = format;
        }

        public override bool CanDisplay => true;

        public override string DeviceName => GetDeviceName(ImageFormat);

        public Display Display { get; set; }

        public ImageFormat ImageFormat { get; }

        public override void AddDisplayTo(ICollection<string> args)
        {
            if (Display != null)
            {
                Display.AddTo(args);
            }
        }

        private static string GetDeviceName(ImageFormat format)
        {
            switch (format)
            {
                case ImageFormat.Bitmap8:
                    return "bmp256";
                case ImageFormat.Bitmap24:
                    return "bmp16m";
                case ImageFormat.BitmapMono:
                    return "bmpgray";
                case ImageFormat.Jpeg:
                    return "jpeg";
                case ImageFormat.Png8:
                    return "png256";
                case ImageFormat.Png24:
                    return "png16m";
                case ImageFormat.PngMono:
                    return "pnggray";
                case ImageFormat.Tiff24:
                    return "tiff24nc";
                case ImageFormat.TiffMono:
                    return "tiffgray";

                case ImageFormat.None:
                default:
                    throw new NotSupportedException("Unsupported format.");
            }
        }
    }
}
