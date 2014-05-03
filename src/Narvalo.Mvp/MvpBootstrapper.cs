// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using Narvalo.Mvp.Configuration;
    using Narvalo.Mvp.Internal;

    public class MvpBootstrapper
    {
        readonly MvpConfiguration _configuration = new MvpConfiguration();

        public MvpBootstrapper() { }

        public MvpConfiguration Configuration { get { return _configuration; } }

        protected virtual void OnDefaultServicesCreated(DefaultServices defaultServices)
        {
        }

        public void Run()
        {
            var defaultServices = new DefaultServices();

            OnDefaultServicesCreated(defaultServices);

            var servicesContainer = _configuration.CreateServicesContainer(defaultServices);

            ServicesContainer.SetContainer(servicesContainer);
        }
    }
}
