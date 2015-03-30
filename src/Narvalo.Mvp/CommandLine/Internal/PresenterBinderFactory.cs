// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine.Internal
{
    using Narvalo;
    using Narvalo.Mvp;
    using Narvalo.Mvp.CommandLine;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;

    internal static class PresenterBinderFactory
    {
        public static PresenterBinder Create(MvpCommand command)
        {
            return Create(command, PlatformServices.Current);
        }

        public static PresenterBinder Create(
            ICommand command,
            IPlatformServices platformServices)
        {
            Require.NotNull(platformServices, "platformServices");

            return new PresenterBinder(
                new[] { command },
                platformServices.PresenterDiscoveryStrategy,
                platformServices.PresenterFactory,
                platformServices.CompositeViewFactory,
                MessageCoordinator.BlackHole);
        }
    }
}
