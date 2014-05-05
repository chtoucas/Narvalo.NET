// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class MessageBus : IMessageBus
    {
        readonly ConcurrentDictionary<Type, IList<Action<object>>> _handlers
            = new ConcurrentDictionary<Type, IList<Action<object>>>();

        public void Publish<T>(T message)
        {
            var messageType = typeof(T);

            var handlersForT = from t in _handlers.Keys
                               where t.IsAssignableFrom(messageType)
                               from handler in _handlers[t]
                               select handler;

            foreach (var handler in handlersForT) {
                handler(message);
            }
        }

        public IObservable<T> Register<T>()
        {
            throw new NotSupportedException("FIXME");
        }

        public void Subscribe<T>(Action<T> onNext)
        {
            Require.NotNull(onNext, "onNext");

            var handlersForT = _handlers.GetOrAdd(typeof(T), _ => new List<Action<object>>());

            lock (handlersForT) {
                handlersForT.Add(_ => onNext((T)_));
            }
        }
    }
}