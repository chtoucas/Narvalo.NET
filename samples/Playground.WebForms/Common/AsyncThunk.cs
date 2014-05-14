namespace Playground.WebForms.Common
{
    using System;
    using System.Threading;

    public static class AsyncThunk
    {
        public static readonly Func<string> A = () =>
        {
            Thread.Sleep(50);
            return "Async task A processed";
        };

        public static readonly Func<string> B = () =>
        {
            Thread.Sleep(50);
            return "Async task B processed";
        };
    }
}