// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using Narvalo;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;

    public static class PresenterBinderFactory
    {
        public static PresenterBinder Create(MvpCommand command)
        {
            return Create(command, PlatformServices.Current);
        }

        internal static PresenterBinder Create(
            ICommand command,
            IPlatformServices platformServices)
        {
            DebugCheck.NotNull(platformServices);

            return new PresenterBinder(
                new[] { command },
                platformServices.PresenterDiscoveryStrategy,
                platformServices.PresenterFactory,
                platformServices.CompositeViewFactory,
                MessageCoordinator.BlackHole);
        }
    }
}
