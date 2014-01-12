namespace Narvalo.GhostScript.Options
{
    public class ImageArgs : GhostScriptArgs<ImageDevice>
    {
        public ImageArgs(string inputFile, ImageFormat format)
            : base(inputFile, new ImageDevice(format))
        {
        }

        public Display Display
        {
            get { return Device.Display; }
            set { Device.Display = value; }
        }
    }
}
