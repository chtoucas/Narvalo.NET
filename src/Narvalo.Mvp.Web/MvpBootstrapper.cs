// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System.ComponentModel;
    using Narvalo.Mvp.Configuration;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Web.Core;

    /// <summary>
    /// Provides a single entry point to configure Narvalo.Mvp.Web.
    /// </summary>
    public sealed class MvpBootstrapper : MvpConfiguration<MvpBootstrapper>
    {
        readonly IAspNetPlatformServices _defaultServices;

        IMessageCoordinatorFactory _messageCoordinatorFactory;

        public MvpBootstrapper() : this(new DefaultAspNetPlatformServices()) { }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public MvpBootstrapper(IAspNetPlatformServices defaultServices)
            : base(defaultServices)
        {
            Require.NotNull(defaultServices, "defaultServices");

            _defaultServices = defaultServices;
        }

        public Setter<MvpBootstrapper, IMessageCoordinatorFactory> MessageCoordinatorFactory
        {
            get
            {
                return new Setter<MvpBootstrapper, IMessageCoordinatorFactory>(
                    this, _ => _messageCoordinatorFactory = _);
            }
        }

        public void Run()
        {
            AspNetPlatformServices.Current = CreateAspNetPlatformServices();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
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
