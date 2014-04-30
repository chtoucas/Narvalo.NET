// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Mvp.Binder;

    internal sealed class BindingServices
    {
        readonly ServiceProvider<ICompositeViewFactory> _compositeViewFactoryProvider
            = new ServiceProvider<ICompositeViewFactory>(() => new DefaultCompositeViewFactory());

        readonly ServiceProvider<IPresenterDiscoveryStrategy> _presenterDiscoveryStrategyProvider
            = new ServiceProvider<IPresenterDiscoveryStrategy>(() => new DefaultPresenterDiscoveryStrategy());

        readonly ServiceProvider<IPresenterFactory> _presenterFactoryProvider
            = new ServiceProvider<IPresenterFactory>(() => new DefaultPresenterFactory());

        BindingServices() { }

        public static BindingServices Current { get { return Singleton.Instance_; } }

        public ICompositeViewFactory CompositeViewFactory
        {
            get { return _compositeViewFactoryProvider.Service; }
            set { _compositeViewFactoryProvider.SetService(value); }
        }

        public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy
        {
            get { return _presenterDiscoveryStrategyProvider.Service; }
            set { _presenterDiscoveryStrategyProvider.SetService(value); }
        }

        public IPresenterFactory PresenterFactory
        {
            get { return _presenterFactoryProvider.Service; }
            set { _presenterFactoryProvider.SetService(value); }
        }

        // See http://csharpindepth.com/articles/general/singleton.aspx
        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
        class Singleton
        {
            [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
            static Singleton() { }

            internal static readonly BindingServices Instance_ = new BindingServices();
        }
    }
}
