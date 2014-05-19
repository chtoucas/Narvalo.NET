namespace Playground.Views
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public sealed class AsyncModel
    {
        readonly List<string> _messages = new List<string>();

        public IEnumerable<string> Messages { get { return _messages; } }

        public void RecordViewLoad()
        {
            Append_("View Load");
        }

        public void RecordAsyncStarted()
        {
            Append_("Async Started");
        }

        public void RecordAsyncEnded()
        {
            Append_("Async Ended");
        }

        public void RecordPagePreRenderComplete()
        {
            Append_("Page PreRenderComplete");
        }

        void Append_(string message)
        {
            _messages.Add(Format_(message));
        }

        static string Format_(string message)
        {
            return String.Format(
                "{2} [Thread={1}] {0}",
                message,
                Thread.CurrentThread.ManagedThreadId,
                DateTime.Now.ToString(@"[HH:mm:ss fff\m\s]"));
        }
    }
}