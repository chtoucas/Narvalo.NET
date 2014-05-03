// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class DefaultServices : IServicesContainer
    {
        readonly Delayed<ICompositeViewFactory> _compositeViewFactory
            = new Delayed<ICompositeViewFactory>(() => new DefaultCompositeViewFactory());

        readonly Delayed<IMessageBus> _messageBus
            = new Delayed<IMessageBus>(() => new MessageBus());

        readonly Delayed<IPresenterDiscoveryStrategy> _presenterDiscoveryStrategy
            = new Delayed<IPresenterDiscoveryStrategy>(() => new AttributeBasedPresenterDiscoveryStrategy());

        readonly Delayed<IPresenterFactory> _presenterFactory
            = new Delayed<IPresenterFactory>(() => new DefaultPresenterFactory());

        public ICompositeViewFactory CompositeViewFactory
        {
            get { return _compositeViewFactory.Value; }
            set { Require.Property(value); _compositeViewFactory.Reset(value); }
        }

        public IMessageBus MessageBus
        {
            get { return _messageBus.Value; }
            set { Require.Property(value); _messageBus.Reset(value); }
        }

        public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy
        {
            get { return _presenterDiscoveryStrategy.Value; }
            set { Require.Property(value); _presenterDiscoveryStrategy.Reset(value); }
        }

        public IPresenterFactory PresenterFactory
        {
            get { return _presenterFactory.Value; }
            set { Require.Property(value); _presenterFactory.Reset(value); }
        }

        public void SetCompositeViewFactory(Func<ICompositeViewFactory> thunk)
        {
            Require.NotNull(thunk, "thunk");

            _compositeViewFactory.Reset(thunk);
        }

        public void SetMessageBus(Func<IMessageBus> thunk)
        {
            Require.NotNull(thunk, "thunk");

            _messageBus.Reset(thunk);
        }

        public void SetPresenterDiscoveryStrategy(Func<IPresenterDiscoveryStrategy> thunk)
        {
            Require.NotNull(thunk, "thunk");

            _presenterDiscoveryStrategy.Reset(thunk);
        }

        public void SetPresenterFactory(Func<IPresenterFactory> thunk)
        {
            Require.NotNull(thunk, "thunk");

            _presenterFactory.Reset(thunk);
        }

        class Delayed<TValue>
        {
            readonly Lazy<TValue> _lazyValue;

            Func<TValue> _valueFactory;

            public Delayed(Func<TValue> valueFactory)
            {
                DebugCheck.NotNull(valueFactory);

                _valueFactory = valueFactory;
                // WARNING: Do not change the following line for:
                // _lazyValue = new Lazy<TValue>(_valueFactory);
                // as it will fail to capture the variable "_valueFactory".
                _lazyValue = new Lazy<TValue>(() => _valueFactory());
            }

            public TValue Value { get { return _lazyValue.Value; } }

            public bool CanReset { get { return !_lazyValue.IsValueCreated; } }

            public void Reset(TValue value)
            {
                Reset(() => value);
            }

            public void Reset(Func<TValue> valueFactory)
            {
                DebugCheck.NotNull(valueFactory);

                if (!CanReset) {
                    throw new InvalidOperationException(
                        "Once accessed, you can no longer change the underlying value factory.");
                }

                // REVIEW: We break thread-safety here, but does it matter for our use case?
                _valueFactory = valueFactory;
            }
        }
    }
}
