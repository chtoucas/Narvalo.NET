// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Options
{
    public sealed class ImageArgs : GhostScriptArgs<ImageDevice>
    {
        public ImageArgs(string inputFile, ImageFormat format)
            : base(inputFile, new ImageDevice(format)) { }

        public Display Display
        {
            get { return Device.Display; }
            set { Device.Display = value; }
        }
    }
}
