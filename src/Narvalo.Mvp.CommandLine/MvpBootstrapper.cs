// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using System.ComponentModel;
    using Narvalo.Mvp.Configuration;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.CommandLine.Core;

    /// <summary>
    /// Provides a single entry point to configure Narvalo.Mvp.CommandLine.
    /// </summary>
    public sealed class MvpBootstrapper : MvpConfiguration<MvpBootstrapper>
    {
        public MvpBootstrapper() : this(new DefaultCommandsPlatformServices()) { }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public MvpBootstrapper(IPlatformServices defaultServices) : base(defaultServices) { }

        public void Run()
        {
            CommandsPlatformServices.Current = CreatePlatformServices();
        }
    }
}
