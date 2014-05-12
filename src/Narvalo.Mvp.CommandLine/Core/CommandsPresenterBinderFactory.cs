// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine.Core
{
    using Narvalo.Mvp.PresenterBinding;

    public static class CommandsPresenterBinderFactory
    {
        public static PresenterBinder Create(ICommand command)
        {
            return PresenterBinding.PresenterBinderFactory
                .Create(new[] { command }, CommandsPlatformServices.Current);
        }
    }
}
