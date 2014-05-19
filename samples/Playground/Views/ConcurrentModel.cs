namespace Playground.Views
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public sealed class ConcurrentModel
    {
        readonly List<string> _messages = new List<string>();

        public IList<string> Messages { get { return _messages; } }

        public void Append(string message)
        {
            lock (_messages) {
                _messages.Add(Format_(message));
            }
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