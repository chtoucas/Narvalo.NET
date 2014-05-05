// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class MessageBus : IMessageBus
    {
        readonly ConcurrentDictionary<Type, IList> _messages
            = new ConcurrentDictionary<Type, IList>();
        readonly ConcurrentDictionary<Type, IList<Action<object>>> _handlers
            = new ConcurrentDictionary<Type, IList<Action<object>>>();

        public void Publish<T>(T message)
        {
            AddMessage_(message);
            PushMessage_(message);
        }

        public IObservable<T> Register<T>()
        {
            throw new NotSupportedException("FIXME");
        }

        public void Subscribe<T>(Action<T> onNext)
        {
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
            var handlersForT = _handlers.GetOrAdd(typeof(T), _ => new List<Action<object>>());

            lock (handlersForT) {
                handlersForT.Add(_ => onNext((T)_));
            }
        }

        void PushMessage_<T>(T message)
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
    }
}