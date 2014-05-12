// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System.ComponentModel;
    using Narvalo.Mvp.Configuration;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Windows.Forms.Core;

    /// <summary>
    /// Provides a single entry point to configure Narvalo.Mvp.WindowsForms.
    /// </summary>
    public sealed class MvpBootstrapper : MvpConfiguration<MvpBootstrapper>
    {
        readonly IFormsPlatformServices _defaultServices;

        IMessageBus _messageBus;

        public MvpBootstrapper() : this(new DefaultFormsPlatformServices()) { }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public MvpBootstrapper(IFormsPlatformServices defaultServices)
            : base(defaultServices)
        {
            Require.NotNull(defaultServices, "defaultServices");

            _defaultServices = defaultServices;
        }

        public Setter<MvpBootstrapper, IMessageBus> MessageBus
        {
            get
            {
                return new Setter<MvpBootstrapper, IMessageBus>(
                    this, _ => _messageBus = _);
            }
        }

        public void Run()
        {
            FormsPlatformServices.Current = CreateFormsPlatformServices();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IFormsPlatformServices CreateFormsPlatformServices()
        {
            var result = new FormsPlatformServices_(CreatePlatformServices());

            result.MessageBus = _messageBus != null
                ? _messageBus
                : _defaultServices.MessageBus;

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

            public IMessageBus MessageBus { get; set; }

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
