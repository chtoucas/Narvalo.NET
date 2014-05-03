// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using Narvalo.Mvp.Configuration;
    using Narvalo.Mvp.Internal;

    public class MvpBootstrapper
    {
        readonly MvpConfiguration _configuration = new MvpConfiguration();

        public MvpBootstrapper() { }

        public event EventHandler<DefaultServicesCreatedEventArgs> DefaultServicesCreated;
        public event EventHandler<ServicesContainerCreatedEventArgs> ServicesContainerCreated;

        public virtual MvpConfiguration Configuration { get { return _configuration; } }

        public void Run()
        {
            var defaultServices = new DefaultServices();
            OnDefaultServicesCreated(defaultServices);

            var servicesContainer = _configuration.CreateServicesContainer(defaultServices);
            OnContainerCreated(servicesContainer);

            ServicesContainer.InnerSet(servicesContainer);
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
