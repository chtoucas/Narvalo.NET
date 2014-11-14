// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Mvp
{
    using Narvalo.Mvp.Platforms;
    using Narvalo.Web.Mvp.Core;

    /// <summary>
    /// Provides a single entry point to configure Narvalo.Web.Mvp.
    /// </summary>
    public sealed class MvpBootstrapper : MvpBootstrapper<MvpBootstrapper>
    {
        public MvpBootstrapper() : base(PlatformServices.Default) { }

        public void Run()
        {
            PlatformServices.Current = CreatePlatformServices();
        }
    }
}
