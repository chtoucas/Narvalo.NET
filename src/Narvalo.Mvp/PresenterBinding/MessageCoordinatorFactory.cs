// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    public sealed class /*Default*/MessageCoordinatorFactory : IMessageCoordinatorFactory
    {
        public IMessageCoordinator Create()
        {
            return new MessageCoordinator();
        }
    }
}
