// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Configuration
{
    using System.ComponentModel;
    using Narvalo.Mvp.Platforms;

    /// <summary>
    /// Provides a single entry point to configure Narvalo.Mvp.
    /// </summary>
    public sealed class MvpBootstrapper : MvpConfiguration<MvpBootstrapper>
    {
        public MvpBootstrapper() : this(new DefaultPlatformServices()) { }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public MvpBootstrapper(IPlatformServices defaultServices) : base(defaultServices) { }

        public void Run()
        {
            PlatformServices.Current = CreatePlatformServices();
        }
    }
}
