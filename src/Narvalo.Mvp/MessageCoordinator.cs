// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class /*Default*/MessageCoordinator : IMessageCoordinator
    {
        readonly ConcurrentDictionary<Type, IList<Action<object>>> _handlers
            = new ConcurrentDictionary<Type, IList<Action<object>>>();
        readonly Object _lock = new Object();

        bool _closed = false;

        public void Close()
        {
            if (_closed) { return; }

            lock (_lock) {
                if (_closed) { return; }

                _closed = true;
            }
        }

        public void Publish<T>(T message)
        {
            ThrowIfClosed_();

            var messageType = typeof(T);

            var handlersForT = from t in _handlers.Keys
                               where t.IsAssignableFrom(messageType)
                               from handler in _handlers[t]
                               select handler;

            foreach (var handler in handlersForT) {
                handler(message);
            }
        }

        public void Subscribe<T>(Action<T> onNext)
        {
            ThrowIfClosed_();

            Require.NotNull(onNext, "onNext");

            var handlersForT = _handlers.GetOrAdd(typeof(T), _ => new List<Action<object>>());

            lock (handlersForT) {
                handlersForT.Add(_ => onNext((T)_));
            }
        }

        void ThrowIfClosed_()
        {
            if (_closed) {
                throw new InvalidOperationException(
                    "Messages can't be published or subscribed to after the message bus has been closed. In a typical page lifecycle, this happens during PreRenderComplete.");
            }
        }
    }
}