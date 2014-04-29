// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    // TODO: To be entirely rewritten.
    // TODO: Make it available to DI.
    public sealed class MessageBus : IMessageBus
    {
        readonly ConcurrentDictionary<Type, IList> _messages
            = new ConcurrentDictionary<Type, IList>();
        readonly ConcurrentDictionary<Type, IList<Action<object>>> _messageReceivedCallbacks
            = new ConcurrentDictionary<Type, IList<Action<object>>>();
        readonly ConcurrentDictionary<Type, IList<Action>> _neverReceivedCallbacks
            = new ConcurrentDictionary<Type, IList<Action>>();

        readonly Object _lock = new Object();

        bool _closed;

        public void Publish<TMessage>(TMessage message)
        {
            ThrowIfClosed_();

            AddMessage_(message);
            PushMessage_(message);
        }

        public void Subscribe<TMessage>(Action<TMessage> messageReceivedCallback)
        {
            Subscribe(messageReceivedCallback, null);
        }

        public void Subscribe<TMessage>(Action<TMessage> messageReceivedCallback, Action neverReceivedCallback)
        {
            ThrowIfClosed_();

            Require.NotNull(messageReceivedCallback, "messageReceivedCallback");

            AddMessageReceivedCallback_(messageReceivedCallback);
            AddNeverReceivedCallback_<TMessage>(neverReceivedCallback);
            PushPreviousMessages_(messageReceivedCallback);
        }

        public void Close()
        {
            if (_closed) { return; }

            lock (_lock) {
                if (_closed) { return; }

                _closed = true;
            }

            FireNeverReceivedCallbacks_();
        }

        void AddMessage_<TMessage>(TMessage message)
        {
            var messageList = _messages.GetOrAdd(typeof(TMessage), _ => new List<TMessage>());

            lock (messageList) {
                messageList.Add(message);
            }
        }

        void PushMessage_<TMessage>(TMessage message)
        {
            var messageType = typeof(TMessage);

            var callbackTypes = _messageReceivedCallbacks
                .Keys
                .Where(k => k.IsAssignableFrom(messageType));

            var callbacks = callbackTypes
                .SelectMany(t => _messageReceivedCallbacks[t])
                .ToArray();

            foreach (var callback in callbacks) {
                callback(message);
            }
        }

        void AddMessageReceivedCallback_<TMessage>(Action<TMessage> messageReceivedCallback)
        {
            var intermediateReceivedCallback = new Action<object>(m =>
                messageReceivedCallback((TMessage)m));

            var receivedList = _messageReceivedCallbacks
                .GetOrAdd(typeof(TMessage), _ => new List<Action<object>>());

            lock (receivedList) {
                receivedList.Add(intermediateReceivedCallback);
            }
        }

        void AddNeverReceivedCallback_<TMessage>(Action neverReceivedCallback)
        {
            if (neverReceivedCallback == null) { return; }

            var neverReceivedList = _neverReceivedCallbacks
                .GetOrAdd(typeof(TMessage), _ => new List<Action>());

            lock (neverReceivedList) {
                neverReceivedList.Add(neverReceivedCallback);
            }
        }

        void PushPreviousMessages_<TMessage>(Action<TMessage> messageReceivedCallback)
        {
            var previousMessageTypes = _messages
                .Keys
                .Where(mt => typeof(TMessage).IsAssignableFrom(mt));

            var previousMessages = previousMessageTypes
                .SelectMany(t => _messages[t].Cast<TMessage>())
                .ToArray();

            foreach (var previousMessage in previousMessages) {
                messageReceivedCallback(previousMessage);
            }
        }

        void ThrowIfClosed_()
        {
            if (_closed) {
                throw new InvalidOperationException(
                    "Messages can't be published or subscribed to after the message bus has been closed.");
            }
        }

        void FireNeverReceivedCallbacks_()
        {
            var neverReceivedMessageTypes = _neverReceivedCallbacks
                .Keys
                .Where(neverReceivedMessageType =>
                    !_messages.Keys.Any(neverReceivedMessageType.IsAssignableFrom));

            var callbacks = neverReceivedMessageTypes
                .SelectMany(t => _neverReceivedCallbacks[t]);

            foreach (var callback in callbacks) {
                callback();
            }
        }
    }
}