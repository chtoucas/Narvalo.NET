// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using Narvalo.Mvp.PresenterBinding;

    internal sealed class ServicesContainer : IServicesContainer
    {
        static readonly ServicesContainer Instance_ = new ServicesContainer();

        IServicesContainer _inner = new DefaultServices();

        public static IServicesContainer Current
        {
            get { return Instance_._inner; }
        }

        public ICompositeViewFactory CompositeViewFactory
        {
            get { return Current.CompositeViewFactory; }
        }

        public IMessageBus MessageBus
        {
            get { return Current.MessageBus; }
        }

        public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy
        {
            get { return Current.PresenterDiscoveryStrategy; }
        }

        public IPresenterFactory PresenterFactory
        {
            get { return Current.PresenterFactory; }
        }

        public static void SetContainer(IServicesContainer servicesContainer)
        {
            Instance_._inner = servicesContainer;
        }
    }
}
