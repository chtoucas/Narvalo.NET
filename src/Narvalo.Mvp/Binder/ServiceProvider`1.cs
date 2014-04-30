// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System;
    using Narvalo;

    public class ServiceProvider<TService> where TService : class
    {
        readonly Lazy<TService> _service;

        Func<TService> _serviceFactory;

        internal protected ServiceProvider(Func<TService> serviceFactory)
        {
            DebugCheck.NotNull(serviceFactory);

            _serviceFactory = serviceFactory;
            _service = new Lazy<TService>(() => _serviceFactory());
        }

        public TService Service { get { return _service.Value; } }

        public void SetService(TService service)
        {
            Require.NotNull(service, "service");

            if (_service.IsValueCreated) {
                throw new InvalidOperationException(
                    "Once accessed, you can no longer change the underlying service.");
            }

            _serviceFactory = () => service;
        }
    }
}
