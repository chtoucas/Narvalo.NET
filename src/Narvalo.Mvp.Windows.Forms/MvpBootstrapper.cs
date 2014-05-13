// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System.ComponentModel;
    using Narvalo.Mvp.Configuration;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.Windows.Forms.Core;

    /// <summary>
    /// Provides a single entry point to configure Narvalo.Mvp.WindowsForms.
    /// </summary>
    public sealed class MvpBootstrapper : MvpConfiguration<MvpBootstrapper>
    {
        public MvpBootstrapper() : this(new DefaultFormsPlatformServices()) { }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public MvpBootstrapper(IPlatformServices defaultServices) : base(defaultServices) { }

        public void Run()
        {
            FormsPlatformServices.Current = CreatePlatformServices();
        }
    }
}
