// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using Narvalo.Mvp.Platforms;

    /// <summary>
    /// Provides a single entry point to configure Narvalo.Mvp.WindowsForms.
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
