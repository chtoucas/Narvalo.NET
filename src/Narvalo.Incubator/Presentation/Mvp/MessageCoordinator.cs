namespace Narvalo.Presentation.Mvp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Internal;

    /// <summary>
    /// A default implementation for cross presenter messaging.
    /// </summary>
    public class MessageCoordinator : IMessageCoordinator
    {
        readonly object closeLock = new Object();

        readonly IDictionary<Type, IList> _messages
            = new Dictionary<Type, IList>();
        readonly IDictionary<Type, IList<Action<object>>> _messageReceivedCallbacks
            = new Dictionary<Type, IList<Action<object>>>();
        readonly IDictionary<Type, IList<Action>> _neverReceivedCallbacks
            = new Dictionary<Type, IList<Action>>();

        bool _closed;

        #region IMessageCoordinator

        /// <summary>
        /// Publishes a message to the bus. Any existing subscriptions to this type,
        /// or an assignable type such as a base class or an interface, will be notified
        /// at this time.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message to publish</typeparam>
        /// <param name="message">The message to publish</param>
        public void Publish<TMessage>(TMessage message)
        {
            ThrowIfClosed_();

            AddMessage_(message);
            PushMessage_(message);
        }

        /// <summary>
        /// Registers a subscription to messages of the specified type. Any previously
        /// published messages that are valid for this subscription will be raised
        /// at this time.
        /// </summary>
        /// <typeparam name="TMessage">The type of messages to subscribe to</typeparam>
        /// <param name="messageReceivedCallback">A callback that will be invoked for each message received. This callback will be invoked once per message.</param>
        public void Subscribe<TMessage>(Action<TMessage> messageReceivedCallback)
        {
            Subscribe(messageReceivedCallback, null /* neverReceivedCallback */);
        }

        /// <summary>
        /// Registers a subscription to messages of the specified type. Any previously
        /// published messages that are valid for this subscription will be raised
        /// at this time.
        /// </summary>
        /// <typeparam name="TMessage">The type of messages to subscribe to</typeparam>
        /// <param name="messageReceivedCallback">A callback that will be invoked for each message received. This callback will be invoked once per message.</param>
        /// <param name="neverReceivedCallback">A callback that will be invoked if no matching message is ever received. This callback will not be invoked more than once.</param>
        public void Subscribe<TMessage>(
            Action<TMessage> messageReceivedCallback, 
            Action neverReceivedCallback)
        {
            ThrowIfClosed_();

            Requires.NotNull(messageReceivedCallback, "messageReceivedCallback");

            AddMessageReceivedCallback_(messageReceivedCallback);
            AddNeverReceivedCallback_<TMessage>(neverReceivedCallback);
            PushPreviousMessages_(messageReceivedCallback);
        }

        /// <summary>
        /// <para>
        ///     Closes the message bus, causing any subscribers that have not yet received
        ///     a message to have their "never received" callback fired.
        /// </para>
        /// <para>
        ///     After this method is called, any further calls to <see cref="IMessageBus.Publish{TMessage}"/> or
        ///     <see cref="IMessageBus.Subscribe{TMessage}(System.Action{TMessage})"/> will result in an
        ///     <see cref="InvalidOperationException"/>.
        /// </para>
        /// <para>
        ///     The <see cref="IMessageCoordinator.Close"/> method may be called multiple times and must not
        ///     fail in this scenario.
        /// </para>
        /// </summary>
        public void Close()
        {
            if (_closed) { return; }

            lock (closeLock) {
                if (_closed) { return; }
                _closed = true;
            }

            FireNeverReceivedCallbacks_();
        }

        #endregion

        #region Membres privés

        void AddMessage_<TMessage>(TMessage message)
        {
            var messages = _messages.GetOrCreateValue(typeof(TMessage), () => new List<TMessage>());

            lock (messages) {
                messages.Add(message);
            }
        }

        void AddMessageReceivedCallback_<TMessage>(Action<TMessage> messageReceivedCallback)
        {
            var intermediateReceivedCallback = new Action<object>(m =>
                messageReceivedCallback((TMessage)m));

            var callbacks = _messageReceivedCallbacks
                .GetOrCreateValue(typeof(TMessage), () => new List<Action<object>>());

            lock (callbacks) {
                callbacks.Add(intermediateReceivedCallback);
            }
        }

        void AddNeverReceivedCallback_<TMessage>(Action neverReceivedCallback)
        {
            if (neverReceivedCallback == null) { return; }

            var callbacks = _neverReceivedCallbacks
                .GetOrCreateValue(typeof(TMessage), () => new List<Action>());

            lock (callbacks) {
                callbacks.Add(neverReceivedCallback);
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
                    "Messages can't be published or subscribed to after the message bus has been closed. In a typical page lifecycle, this happens during PreRenderComplete.");
            }
        }

        #endregion
    }
}