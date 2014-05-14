// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public class /*Default*/MessageCoordinator : IMessageCoordinator
    {
        readonly bool _closeable;
        readonly Object _lock = new Object();

        bool _closed = false;

        public MessageCoordinator() : this(closeable: true) { }

        protected MessageCoordinator(bool closeable)
        {
            _closeable = closeable;
        }

        public static MessageCoordinator BlackHole { get { return BlackHole_.Instance; } }

        public void Close()
        {
            if (!_closeable || _closed) { return; }

            lock (_lock) {
                if (_closed) { return; }

                _closed = true;
            }
        }

        public void Publish<T>(T message)
        {
            ThrowIfClosed_();

            PublishCore(message);
        }

        public void Subscribe<T>(Action<T> onNext)
        {
            ThrowIfClosed_();

            SubscribeCore(onNext);
        }

        protected virtual void PublishCore<T>(T message)
        {
            __Tracer.Warning(typeof(MessageCoordinator),
                "All messages published to this bus are dropped.");
        }

        protected virtual void SubscribeCore<T>(Action<T> onNext)
        {
            __Tracer.Warning(typeof(MessageCoordinator),
                "Even if subscription is allowed, no messages will be ever received.");
        }

        void ThrowIfClosed_()
        {
            if (_closed) {
                throw new InvalidOperationException(
                    "Messages can't be published or subscribed to after the message bus has been closed.");
            }
        }

        class BlackHole_ : MessageCoordinator
        {
            // NB: Since we choose a singleton, we disable the ability to close this message bus.
            BlackHole_() : base(closeable: false) { }

            public static BlackHole_ Instance { get { return Singleton.Instance; } }

            protected override void PublishCore<T>(T message) { }

            protected override void SubscribeCore<T>(Action<T> onNext) { }

            [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
                Justification = "Implementation of lazy initialized singleton.")]
            class Singleton
            {
                // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit.
                [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline",
                   Justification = "Implementation of lazy initialized singleton.")]
                static Singleton() { }

                internal static readonly BlackHole_ Instance
                    = new BlackHole_();
            }
        }
    }
}
