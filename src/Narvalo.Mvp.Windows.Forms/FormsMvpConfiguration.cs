// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using Narvalo.Mvp.Configuration;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;

    /// <summary>
    /// Provides a single entry point to configure Narvalo.Mvp.WindowsForms.
    /// </summary>
    public sealed class FormsMvpConfiguration : MvpConfiguration
    {
        readonly IFormsPlatformServices _defaultServices;

        IMessageBusFactory _messageBusFactory;

        public FormsMvpConfiguration() : this(new DefaultFormsPlatformServices()) { }

        public FormsMvpConfiguration(IFormsPlatformServices defaultServices)
            : base(defaultServices)
        {
            Require.NotNull(defaultServices, "defaultServices");

            _defaultServices = defaultServices;
        }

        public Setter<MvpConfiguration, IMessageBusFactory> MessageBusFactory
        {
            get
            {
                return new Setter<MvpConfiguration, IMessageBusFactory>(
                    this, _ => _messageBusFactory = _);
            }
        }

        public IFormsPlatformServices CreateFormsPlatformServices()
        {
            var result = new FormsPlatformServices_(CreatePlatformServices());

            result.MessageBusFactory = _messageBusFactory != null
                ? _messageBusFactory
                : _defaultServices.MessageBusFactory;

            return result;
        }

        class FormsPlatformServices_ : IFormsPlatformServices
        {
            readonly IPlatformServices _inner;

            public FormsPlatformServices_(IPlatformServices inner)
            {
                _inner = inner;
            }

            public ICompositeViewFactory CompositeViewFactory
            {
                get { return _inner.CompositeViewFactory; }
            }

            public IMessageBusFactory MessageBusFactory { get; set; }

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
