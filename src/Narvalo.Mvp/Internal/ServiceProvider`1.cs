// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;
    using Narvalo;

    public class ServiceProvider<TService>
    {
        readonly Lazy<TService> _service;

        Func<TService> _serviceFactory;

        protected ServiceProvider(Func<TService> serviceFactory)
        {
            Require.NotNull(serviceFactory, "serviceFactory");

            _serviceFactory = serviceFactory;
            _service = new Lazy<TService>(() => _serviceFactory());
        }

        public TService Service { get { return _service.Value; } }

        public void SetService(TService service)
        {
            if (_service.IsValueCreated) {
                throw new InvalidOperationException(
                    "Once accessed, you can no longer change the underlying service.");
            }

            _serviceFactory = () => service;
        }
    }
}
