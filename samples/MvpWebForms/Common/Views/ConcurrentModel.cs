// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Views
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    public sealed class ConcurrentModel
    {
        readonly List<string> _messages = new List<string>();

        public IList<string> Messages { get { return _messages; } }

        public void Append(string message)
        {
            lock (_messages)
            {
                _messages.Add(Format_(message));
            }
        }

        private static string Format_(string message)
        {
            return String.Format(
                CultureInfo.InvariantCulture,
                "[Thread={0}] {1}",
                Thread.CurrentThread.ManagedThreadId,
                message);
        }
    }
}
