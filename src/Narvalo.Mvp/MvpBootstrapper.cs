// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.ComponentModel;
    using Narvalo.Mvp.Configuration;
    using Narvalo.Mvp.Services;

    public class MvpBootstrapper
    {
        readonly MvpConfiguration _configuration;

        public event EventHandler<DefaultServicesCreatedEventArgs> DefaultServicesCreated;
        public event EventHandler<ServicesContainerCreatedEventArgs> ServicesContainerCreated;

        public MvpBootstrapper() : this(new MvpConfiguration()) { }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public MvpBootstrapper(MvpConfiguration configuration)
        {
            Require.NotNull(configuration, "configuration");

            _configuration = configuration;
        }

        public MvpConfiguration Configuration { get { return _configuration; } }

        public void Run()
        {
            var defaultServices = new DefaultServices();
            OnDefaultServicesCreated(defaultServices);

            var servicesContainer = _configuration.CreateServicesContainer(defaultServices);
            OnContainerCreated(servicesContainer);

            GlobalServicesContainer.InnerSet(servicesContainer);
        }

        protected virtual void OnDefaultServicesCreated(DefaultServices defaultServices)
        {
            var localHandler = DefaultServicesCreated;

            if (localHandler != null) {
                localHandler(this, new DefaultServicesCreatedEventArgs(defaultServices));
            }
        }

        protected virtual void OnContainerCreated(IServicesContainer container)
        {
            var localHandler = ServicesContainerCreated;

            if (localHandler != null) {
                localHandler(this, new ServicesContainerCreatedEventArgs(container));
            }
        }
    }
}
