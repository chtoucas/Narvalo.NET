namespace Narvalo.Runtime.Reliability
{
    using System;

    public class BlackholeGuard : IGuard
    {
        #region IGuard

        public void Execute(Action action)
        {
            ;
        }

        #endregion
    }
}
