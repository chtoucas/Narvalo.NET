// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;
    using Narvalo;

    internal class ServiceProvider<TService> where TService : class
    {
        readonly Lazy<TService> _service;

        Func<TService> _serviceFactory;

        public ServiceProvider(Func<TService> serviceFactory)
        {
            DebugCheck.NotNull(serviceFactory);

            _serviceFactory = serviceFactory;
            _service = new Lazy<TService>(() => _serviceFactory());
        }

        public TService Service { get { return _service.Value; } }

        public void SetService(TService service)
        {
            DebugCheck.NotNull(service);

            if (_service.IsValueCreated) {
                throw new InvalidOperationException(
                    "Once accessed, you can no longer change the underlying service.");
            }

            _serviceFactory = () => service;
        }
    }
}
