namespace Narvalo.Internal
{
    using System;
    using System.Threading;

    public class CompletedAsyncResult : IAsyncResult
    {
        #region IAsyncResult

        public object AsyncState
        {
            get { return null; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return null; }
        }

        public bool CompletedSynchronously
        {
            get { return true; }
        }

        public bool IsCompleted
        {
            get { return true; }
        }

        #endregion
    }
}
