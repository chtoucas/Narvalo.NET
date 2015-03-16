// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Narvalo.Mvp;

    // Only views bound by the same "HttpPresenteBinder" will share this message bus.
    // In practice, cross-presenter communication is applicable to all presenters
    // activated during the same HTTP request.
    // When a message is published, it will be handled by all present AND future handlers.
    // You then do not have to worry about the order of the pub/sub events.
    // A good rule of thumb is to subscribe or publish messages during the Load event. 
    // NB: After pre-render completes, the message bus is closed.
    public sealed class AspNetMessageCoordinator : IMessageCoordinator
    {
        private readonly ConcurrentDictionary<Type, IList> _messages
            = new ConcurrentDictionary<Type, IList>();

        private readonly ConcurrentDictionary<Type, IList<Action<object>>> _handlers
            = new ConcurrentDictionary<Type, IList<Action<object>>>();

        private readonly Object _lock = new Object();

        private bool _closed = false;

        public void Close()
        {
            if (_closed) { return; }

            lock (_lock)
            {
                if (_closed) { return; }

                _closed = true;
            }

            __TraceNeverReceivedMessages();
        }

        public void Publish<T>(T message)
        {
            ThrowIfClosed_();

            AddMessage_(message);
            PushMessage_(message);
        }

        public void Subscribe<T>(Action<T> onNext)
        {
            ThrowIfClosed_();

            Require.NotNull(onNext, "onNext");

            AddHandler_(onNext);
            PushPreviousMessages_(onNext);
        }

        private void AddMessage_<T>(T message)
        {
            var messagesOfT = _messages.GetOrAdd(typeof(T), _ => new List<T>());

            lock (messagesOfT)
            {
                messagesOfT.Add(message);
            }
        }

        private void AddHandler_<T>(Action<T> onNext)
        {
            var handlersOfT = _handlers.GetOrAdd(typeof(T), _ => new List<Action<object>>());

            lock (handlersOfT)
            {
                handlersOfT.Add(_ => onNext((T)_));
            }
        }

        private void PushMessage_<T>(T message)
        {
            var messageType = typeof(T);

            var handlersOfT = from t in _handlers.Keys
                              where t.IsAssignableFrom(messageType)
                              from handler in _handlers[t]
                              select handler;

            foreach (var handler in handlersOfT)
            {
                handler(message);
            }
        }

        private void PushPreviousMessages_<T>(Action<T> onNext)
        {
            var messageType = typeof(T);

            var messagesOfT = from t in _messages.Keys
                              where messageType.IsAssignableFrom(t)
                              from m in _messages[t].Cast<T>()
                              select m;

            foreach (var message in messagesOfT)
            {
                onNext(message);
            }
        }

        [Conditional("TRACE")]
        private void __TraceNeverReceivedMessages()
        {
            var neverReceivedMessages = _messages.Keys.Except(_handlers.Keys);

            foreach (var type in neverReceivedMessages)
            {
                Trace.TraceWarning(
                    "[AspNetMessageCoordinator] You published a message of type '{1}' but you did not registered any handler for it.",
                    type.FullName);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "PreRenderComplete",
            Justification = "ASP.NET method name.")]
        private void ThrowIfClosed_()
        {
            if (_closed)
            {
                throw new InvalidOperationException(
                    "Messages can't be published or subscribed to after the message bus has been closed. In a typical page lifecycle, this happens during 'PreRenderComplete'.");
            }
        }
    }
}