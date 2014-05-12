// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms.Core
{
    using System.Windows.Forms;

    public static class FormsPresenterBinderFactory
    {
        public static FormsPresenterBinder Create<T>(T host)
            where T : Control
        {
            return Create(host, FormsPlatformServices.Current);
        }

        public static FormsPresenterBinder Create<T>(
            T host,
            IFormsPlatformServices container)
            where T : Control
        {
            Require.NotNull(container, "container");

            return new FormsPresenterBinder(
                host,
                container.PresenterDiscoveryStrategy,
                container.PresenterFactory,
                container.CompositeViewFactory,
                container.MessageBus);
        }
    }
}
