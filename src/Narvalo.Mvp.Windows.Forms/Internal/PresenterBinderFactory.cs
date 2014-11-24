// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms.Internal
{
    using System.Windows.Forms;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;

    internal static class PresenterBinderFactory
    {
        public static PresenterBinder Create(MvpForm form)
        {
            return Create(
                form,
                PlatformServices.Current,
                PlatformServices.Current.MessageCoordinatorFactory.Create());
        }

        public static PresenterBinder Create(MvpUserControl control)
        {
            return Create(
                control, 
                PlatformServices.Current, 
                MessageCoordinator.BlackHole);
        }

        public static PresenterBinder Create(
            Control control,
            IPlatformServices platformServices,
            IMessageCoordinator messageCoordinator)
        {
            Require.NotNull(platformServices, "platformServices");

            return new PresenterBinder(
                new[] { control },
                platformServices.PresenterDiscoveryStrategy,
                platformServices.PresenterFactory,
                platformServices.CompositeViewFactory,
                messageCoordinator);
        }
    }
}
