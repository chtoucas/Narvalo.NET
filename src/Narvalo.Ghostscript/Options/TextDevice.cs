namespace Narvalo.GhostScript.Options
{
    public class TextDevice : OutputDevice
    {
        public TextDevice() : base() { }

        public override bool CanDisplay { get { return false; } }

        public override string DeviceName
        {
            get { return "txtwrite"; }
        }
    }
}

