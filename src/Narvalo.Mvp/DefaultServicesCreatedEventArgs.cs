// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;

    public sealed class DefaultServicesCreatedEventArgs : EventArgs
    {
        readonly DefaultServices _defaultServices;

        public DefaultServicesCreatedEventArgs(DefaultServices defaultServices)
        {
            _defaultServices = defaultServices;
        }

        public DefaultServices DefaultServices { get { return _defaultServices; } }
    }
}
