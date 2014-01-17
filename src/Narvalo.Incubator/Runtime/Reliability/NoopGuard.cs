namespace Narvalo.Runtime.Reliability
{
    using System;

    public class NoopGuard : IGuard
    {
        #region IGuard

        public void Execute(Action action)
        {
            Require.NotNull(action, "action");

            action();
        }

        #endregion
    }
}
