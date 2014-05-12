// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public sealed class /*Default*/MessageCoordinator : IMessageCoordinator
    {
        readonly ConcurrentDictionary<Type, IList> _messages
            = new ConcurrentDictionary<Type, IList>();
        readonly ConcurrentDictionary<Type, IList<Action<object>>> _handlers
            = new ConcurrentDictionary<Type, IList<Action<object>>>();

        bool _disposed = false;

        public void Close()
        {
            __TraceNeverReceivedMessages();

            Dispose_(true);
        }

        public void Publish<T>(T message)
        {
            ThrowIfDisposed_();

            AddMessage_(message);
            PushMessage_(message);
        }

        public void Subscribe<T>(Action<T> onNext)
        {
            ThrowIfDisposed_();

            Require.NotNull(onNext, "onNext");

            AddHandler_(onNext);
            PushPreviousMessages_(onNext);
        }

        void AddMessage_<T>(T message)
        {
            var messagesOfT = _messages.GetOrAdd(typeof(T), _ => new List<T>());

            lock (messagesOfT) {
                messagesOfT.Add(message);
            }
        }

        void AddHandler_<T>(Action<T> onNext)
        {
            var handlersOfT = _handlers.GetOrAdd(typeof(T), _ => new List<Action<object>>());

            lock (handlersOfT) {
                handlersOfT.Add(_ => onNext((T)_));
            }
        }

        void PushMessage_<T>(T message)
        {
            var messageType = typeof(T);

            var handlersOfT = from t in _handlers.Keys
                              where t.IsAssignableFrom(messageType)
                              from handler in _handlers[t]
                              select handler;

            foreach (var handler in handlersOfT) {
                handler(message);
            }
        }

        void PushPreviousMessages_<T>(Action<T> onNext)
        {
            var messageType = typeof(T);

            var messagesOfT = from t in _messages.Keys
                              where messageType.IsAssignableFrom(t)
                              from m in _messages[t].Cast<T>()
                              select m;

            foreach (var message in messagesOfT) {
                onNext(message);
            }
        }

        [Conditional("TRACE")]
        void __TraceNeverReceivedMessages()
        {
            var neverReceivedMessages = _messages.Keys.Except(_handlers.Keys);

            foreach (var type in neverReceivedMessages) {
                __Tracer.Warning(
                    typeof(MessageCoordinator),
                    @"You published a message of type ""{0}"" but you did not registered any handler for it.",
                    type.FullName);
            }
        }

        void ThrowIfDisposed_()
        {
            if (_disposed) {
                throw new ObjectDisposedException(
                    typeof(MessageCoordinator).FullName,
                    "Messages can't be published or subscribed to after the message bus has been closed. In a typical page lifecycle, this happens during PreRenderComplete.");
            }
        }

        public void Dispose()
        {
            Dispose_(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        void Dispose_(bool disposing)
        {
            if (!_disposed) {
                if (disposing) {
                    // REVIEW
                    _handlers.Clear();
                    _messages.Clear();
                }

                _disposed = true;
            }
        }
    }
}