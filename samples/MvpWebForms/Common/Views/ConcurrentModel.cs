namespace MvpWebForms.Views
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
            return String.Format("[Thread={0}] {1}", Thread.CurrentThread.ManagedThreadId, message);
        }
    }
}