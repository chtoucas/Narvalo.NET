namespace Narvalo.GhostScript.Options
{
    using System;
    using System.Collections.Generic;

    public abstract class Device
    {
        protected Device() { }

        public virtual bool CanDisplay { get { return false; } }

        public virtual void AddDisplayTo(ICollection<string> args)
        {
            throw new NotSupportedException();
        }

        public abstract void AddTo(ICollection<string> args);
    }
}

