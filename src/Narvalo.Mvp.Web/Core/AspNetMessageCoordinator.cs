// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Narvalo.Mvp;
    using Narvalo.Mvp.Web.Properties;

    using static System.Diagnostics.Contracts.Contract;

    // Only views bound by the same "HttpPresenteBinder" will share this message bus.
    // In practice, cross-presenter communication is applicable to all presenters
    // activated during the same HTTP request.
    // When a message is published, it will be handled by all present AND future handlers.
    // You then do not have to worry about the order of the pub/sub events.
    // A good rule of thumb is to subscribe or publish messages during the Load event.
    // NB: After pre-render completes, the message bus is closed.
    public sealed partial class AspNetMessageCoordinator : IMessageCoordinator
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

            TraceNeverReceivedMessages();
        }

        public void Publish<T>(T message)
        {
            ThrowIfClosed();

            AddMessage(message);
            PushMessage(message);
        }

        public void Subscribe<T>(Action<T> onNext)
        {
            Require.NotNull(onNext, nameof(onNext));

            ThrowIfClosed();

            AddHandler(onNext);
            PushPreviousMessages(onNext);
        }

        private void AddMessage<T>(T message)
        {
            var messagesOfT = _messages.GetOrAdd(typeof(T), _ => new List<T>());
            Assume(messagesOfT != null, "Extern: BCL.");

            lock (messagesOfT)
            {
                messagesOfT.Add(message);
            }
        }

        private void AddHandler<T>(Action<T> onNext)
        {
            Demand.NotNull(onNext);

            var handlersOfT = _handlers.GetOrAdd(typeof(T), _ => new List<Action<object>>());
            Assume(handlersOfT != null, "Extern: BCL.");

            lock (handlersOfT)
            {
                handlersOfT.Add(_ => onNext.Invoke((T)_));
            }
        }

        private void PushMessage<T>(T message)
        {
            var messageType = typeof(T);

            var handlersOfT = from t in _handlers.Keys
                              where t.IsAssignableFrom(messageType)
                              from handler in _handlers[t]
                              select handler;

            foreach (var handler in handlersOfT)
            {
                // A fortiori, this is not possible, anyway...
                if (handler == null) { continue; }

                handler.Invoke(message);
            }
        }

        private void PushPreviousMessages<T>(Action<T> onNext)
        {
            Demand.NotNull(onNext);

            var messageType = typeof(T);

            var messagesOfT = from t in _messages.Keys
                              where messageType.IsAssignableFrom(t)
                              from m in _messages[t].Cast<T>()
                              select m;

            foreach (var message in messagesOfT)
            {
                onNext.Invoke(message);
            }
        }

        [Conditional("TRACE")]
        private void TraceNeverReceivedMessages()
        {
            var neverReceivedMessages = _messages.Keys.Except(_handlers.Keys);

            foreach (var type in neverReceivedMessages)
            {
                if (type == null) { continue; }

                Trace.TraceWarning(
                    "[AspNetMessageCoordinator] You published a message of type '{0}' but you did not registered any handler for it.",
                    type.FullName);
            }
        }

        private void ThrowIfClosed()
        {
            if (_closed)
            {
                throw new InvalidOperationException(Strings.AspNetMessageCoordinator_Closed);
            }
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Mvp.Web.Core
{
    using System.Diagnostics.Contracts;

    public sealed partial class AspNetMessageCoordinator
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_handlers != null);
            Contract.Invariant(_lock != null);
            Contract.Invariant(_messages != null);
        }
    }
}

#endif
