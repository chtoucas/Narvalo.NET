// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using Narvalo.Mvp.Configuration;
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Platforms;

    /// <summary>
    /// Provides a single entry point to configure Narvalo.Mvp.Web.
    /// </summary>
    public sealed class AspNetMvpConfiguration : MvpConfiguration
    {
        readonly IAspNetPlatformServices _defaultServices;

        IMessageCoordinatorFactory _messageCoordinatorFactory;

        public AspNetMvpConfiguration() : this(new DefaultAspNetPlatformServices()) { }

        public AspNetMvpConfiguration(IAspNetPlatformServices defaultServices)
            : base(defaultServices)
        {
            Require.NotNull(defaultServices, "defaultServices");

            _defaultServices = defaultServices;
        }

        public Setter<MvpConfiguration, IMessageCoordinatorFactory> MessageCoordinatorFactory
        {
            get
            {
                return new Setter<MvpConfiguration, IMessageCoordinatorFactory>(
                    this, _ => _messageCoordinatorFactory = _);
            }
        }

        public IAspNetPlatformServices CreateAspNetPlatformServices()
        {
            var result = new AspNetPlatformServices_(CreatePlatformServices());

            result.MessageCoordinatorFactory = _messageCoordinatorFactory != null
                ? _messageCoordinatorFactory
                : _defaultServices.MessageCoordinatorFactory;

            return result;
        }

        class AspNetPlatformServices_ : IAspNetPlatformServices
        {
            readonly IPlatformServices _inner;

            public AspNetPlatformServices_(IPlatformServices inner)
            {
                _inner = inner;
            }

            public ICompositeViewFactory CompositeViewFactory
            {
                get { return _inner.CompositeViewFactory; }
            }

            public IMessageCoordinatorFactory MessageCoordinatorFactory { get; set; }

            public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy
            {
                get { return _inner.PresenterDiscoveryStrategy; }
            }

            public IPresenterFactory PresenterFactory
            {
                get { return _inner.PresenterFactory; }
            }
        }
    }
}
