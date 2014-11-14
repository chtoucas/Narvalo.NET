// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Mvp.Core
{
    using Narvalo.Mvp;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class AspNetMessageCoordinatorFactory : IMessageCoordinatorFactory
    {
        public IMessageCoordinator Create()
        {
            return new AspNetMessageCoordinator();
        }
    }
}
