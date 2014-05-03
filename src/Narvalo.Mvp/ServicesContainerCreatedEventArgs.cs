// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;

    public sealed class ServicesContainerCreatedEventArgs : EventArgs
    {
        readonly IServicesContainer _container;

        public ServicesContainerCreatedEventArgs(IServicesContainer container)
        {
            _container = container;
        }

        public IServicesContainer Container { get { return _container; } }
    }
}
