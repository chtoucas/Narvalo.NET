namespace Narvalo.Runtime.Reliability
{
    using System;

    public class NoopGuard : IGuard
    {
        #region IGuard

        public void Execute(Action action)
        {
            Requires.NotNull(action, "action");

            action();
        }

        #endregion
    }
}
