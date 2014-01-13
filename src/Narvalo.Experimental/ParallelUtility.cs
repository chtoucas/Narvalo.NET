namespace Narvalo
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    // http://msdn.microsoft.com/en-us/library/bz33kx67.aspx
    // http://code.msdn.microsoft.com/ParExtSamples

    /// <summary>
    /// Extensions to the EAP (Event-based Asynchronous Pattern)
    /// </summary>
    internal static class ParallelUtility
    {
        public static void HandleCompletion<T>(
            TaskCompletionSource<T> tcs, AsyncCompletedEventArgs e, Func<T> getResult, Action unregisterHandler)
        {
            // Transfers the results from the AsyncCompletedEventArgs and getResult() to the 
            // TaskCompletionSource, but only AsyncCompletedEventArg's UserState matches the TCS 
            // (this check is important if the same WebClient is used for multiple, asynchronous 
            // operations concurrently). Also unregisters the handler to avoid a leak. 
            if (e.UserState == tcs) {
                if (e.Cancelled) {
                    tcs.TrySetCanceled();
                }
                else if (e.Error != null) {
                    tcs.TrySetException(e.Error);
                }
                else {
                    tcs.TrySetResult(getResult());
                }

                unregisterHandler();
            }
        }
    }
}
