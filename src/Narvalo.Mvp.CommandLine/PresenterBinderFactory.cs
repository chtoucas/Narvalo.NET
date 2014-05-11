// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using Narvalo.Mvp.PresenterBinding;

    public static class PresenterBinderFactory
    {
        public static PresenterBinder Create(object host)
        {
            return PresenterBinding.PresenterBinderFactory
                .Create(new[] { host }, CommandsPlatformServices.Current);
        }
    }
}
