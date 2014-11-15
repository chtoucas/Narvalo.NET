// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.Web.Core;

    /// <summary>
    /// Provides a single entry point to configure Narvalo.Mvp.Web.
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
