// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Mvp.Binder;

    internal sealed class BindingServicesContainer
    {
        readonly Delayed<ICompositeViewFactory> _compositeViewFactory
            = new Delayed<ICompositeViewFactory>(() => new DefaultCompositeViewFactory());

        readonly Delayed<IPresenterDiscoveryStrategy> _presenterDiscoveryStrategy
            = new Delayed<IPresenterDiscoveryStrategy>(() => new AttributeBasedPresenterDiscoveryStrategy());

        readonly Delayed<IPresenterFactory> _presenterFactory
            = new Delayed<IPresenterFactory>(() => new DefaultPresenterFactory());

        BindingServicesContainer() { }

        public static BindingServicesContainer Current { get { return Singleton.Instance_; } }

        public ICompositeViewFactory CompositeViewFactory
        {
            get { return _compositeViewFactory.Value; }
            set { DebugCheck.NotNull(value); _compositeViewFactory.Reset(value); }
        }

        public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy
        {
            get { return _presenterDiscoveryStrategy.Value; }
            set { DebugCheck.NotNull(value); _presenterDiscoveryStrategy.Reset(value); }
        }

        public IPresenterFactory PresenterFactory
        {
            get { return _presenterFactory.Value; }
            set { DebugCheck.NotNull(value); _presenterFactory.Reset(value); }
        }

        // See http://csharpindepth.com/articles/general/singleton.aspx
        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
        class Singleton
        {
            [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
            static Singleton() { }

            internal static readonly BindingServicesContainer Instance_ = new BindingServicesContainer();
        }
    }
}
