// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms.Core
{
    using System.ComponentModel;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class FormsPlatformServicesVirtualProxy
        : LazyValueHolder<IFormsPlatformServices>, IFormsPlatformServices
    {
        public FormsPlatformServicesVirtualProxy() : base(() => new DefaultFormsPlatformServices()) { }

        public ICompositeViewFactory CompositeViewFactory
        {
            get { return Value.CompositeViewFactory; }
        }

        public IMessageBus MessageBus
        {
            get { return Value.MessageBus; }
        }

        public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy
        {
            get { return Value.PresenterDiscoveryStrategy; }
        }

        public IPresenterFactory PresenterFactory
        {
            get { return Value.PresenterFactory; }
        }
    }
}
