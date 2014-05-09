// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using Narvalo.Mvp.Configuration;

    public sealed class AspNetMvpConfiguration : MvpConfiguration
    {
        IMessageCoordinatorFactory _messageCoordinatorFactory;

        public Setter<MvpConfiguration, IMessageCoordinatorFactory> MessageCoordinatorFactory
        {
            get
            {
                return new Setter<MvpConfiguration, IMessageCoordinatorFactory>(
                    this, _ => _messageCoordinatorFactory = _);
            }
        }

        public IAspNetServicesContainer CreateServicesContainer(AspNetDefaultServices defaultServices)
        {
            throw new NotImplementedException();
        }
    }
}
