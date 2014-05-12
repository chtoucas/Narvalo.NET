// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using Narvalo.Mvp.Platforms;

    public interface IAspNetPlatformServices : IPlatformServices
    {
        IMessageCoordinatorFactory MessageCoordinatorFactory { get; }
    }
}
