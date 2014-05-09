// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Services
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Mvp.PresenterBinding;

    [EditorBrowsableAttribute(EditorBrowsableState.Never)]
    public sealed class GlobalServicesContainer : IServicesContainer
    {
        LazyLazy<IServicesContainer> _inner
            = new LazyLazy<IServicesContainer>(() => new DefaultServices());

        public GlobalServicesContainer() { }

        static GlobalServicesContainer Current_ { get { return Singleton.Instance_; } }

        public ICompositeViewFactory CompositeViewFactory
        {
            get { return Current_._inner.Value.CompositeViewFactory; }
        }

        public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy
        {
            get { return Current_._inner.Value.PresenterDiscoveryStrategy; }
        }

        public IPresenterFactory PresenterFactory
        {
            get { return Current_._inner.Value.PresenterFactory; }
        }

        public static void InnerSet(IServicesContainer servicesContainer)
        {
            Current_._inner.Reset(servicesContainer);
        }

        // Cf. http://csharpindepth.com/articles/general/singleton.aspx
        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
            Justification = "Singleton pattern.")]
        class Singleton
        {
            // Explicit static constructor to tell C# compiler not to mark type as "beforefieldinit".
            [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline",
                Justification = "Singleton pattern.")]
            static Singleton() { }

            internal static readonly GlobalServicesContainer Instance_ = new GlobalServicesContainer();
        }

        class LazyLazy<TValue>
        {
            readonly Lazy<TValue> _lazyValue;

            Func<TValue> _valueFactory;

            public LazyLazy(Func<TValue> valueFactory)
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
