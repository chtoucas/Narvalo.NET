// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class ServicesContainer : IServicesContainer
    {
        IServicesContainer _inner;

        ServicesContainer() { }

        public static ServicesContainer Current { get { return Singleton.Instance_; } }

        IServicesContainer Inner_
        {
            get { return _inner ?? (_inner = new DefaultServices()); }
        }

        public void Initialize(IServicesContainer servicesContainer)
        {
            _inner = servicesContainer;
        }

        public ICompositeViewFactory CompositeViewFactory
        {
            get { return Inner_.CompositeViewFactory; }
        }

        public IMessageBus MessageBus
        {
            get { return Inner_.MessageBus; }
        }

        public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy
        {
            get { return Inner_.PresenterDiscoveryStrategy; }
        }

        public IPresenterFactory PresenterFactory
        {
            get { return Inner_.PresenterFactory; }
        }

        // See http://csharpindepth.com/articles/general/singleton.aspx
        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
        class Singleton
        {
            [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
            static Singleton() { }

            internal static readonly ServicesContainer Instance_ = new ServicesContainer();
        }
    }
}
