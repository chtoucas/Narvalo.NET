namespace Narvalo.GhostScript.Options
{
    using System.Collections.Generic;

    public abstract class OutputDevice : Device
    {
        bool? _ignoreMultipleCopies;

        protected OutputDevice() : base() { }

        public abstract string DeviceName { get; }

        public bool? IgnoreMultipleCopies
        {
            get { return _ignoreMultipleCopies; }
            set { _ignoreMultipleCopies = value; }
        }

        public override void AddTo(ICollection<string> args)
        {
            Requires.NotNull(args, "args");

            if (IgnoreMultipleCopies.HasValue && IgnoreMultipleCopies.Value) {
                args.Add("-d.IgnoreNumCopies=true");
                args.Add("-sDEVICE=" + DeviceName);
            }
            else {
                args.Add("-sDEVICE=" + DeviceName);
            }
        }
    }
}
