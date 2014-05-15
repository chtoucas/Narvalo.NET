namespace Playground.WebForms.Views.Models
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class AsyncMessagesModel
    {
        readonly Object Lock_ = new Object();

        public IList<string> Messages { get; private set; }

        public AsyncMessagesModel()
        {
            Messages = new List<string>();
        }

        public void Append(string message)
        {
            lock (Lock_) {
                Messages.Add(Format_(message));
            }
        }

        static string Format_(string message)
        {
            return String.Format(
                "{2} [Thread={1}] {0}.",
                message,
                Thread.CurrentThread.ManagedThreadId,
                DateTime.Now.ToString(@"[HH:mm:ss fff\m\s]"));
        }
    }
}