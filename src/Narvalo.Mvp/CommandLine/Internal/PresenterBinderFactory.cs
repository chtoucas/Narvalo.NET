// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine.Internal
{
    using System.Diagnostics.Contracts;

    using Narvalo;
    using Narvalo.Mvp;
    using Narvalo.Mvp.CommandLine;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;

    using static System.Diagnostics.Contracts.Contract;

    internal static class PresenterBinderFactory
    {
        public static PresenterBinder Create(MvpCommand command)
        {
            Expect.NotNull(command);
            Ensures(Result<PresenterBinder>() != null);

            return Create(command, PlatformServices.Current);
        }

        [ContractVerification(false)]   // Strange CCCheck complains with "ensures unreachable".
        // Even a SuppressMessage won't work here, the problem will propagate to MvpCommand.
        public static PresenterBinder Create(
            ICommand command,
            IPlatformServices platformServices)
        {
            Require.NotNull(platformServices, nameof(platformServices));
            Ensures(Result<PresenterBinder>() != null);

            return new PresenterBinder(
                new[] { command },
                platformServices.PresenterDiscoveryStrategy,
                platformServices.PresenterFactory,
                platformServices.CompositeViewFactory,
                MessageCoordinator.BlackHole);
        }
    }
}
