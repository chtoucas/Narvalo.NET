// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using Narvalo.Mvp.Configuration;
    using Narvalo.Mvp.Platforms;

    public sealed class MvpBootstrapper
    {
        readonly FormsMvpConfiguration _configuration = new FormsMvpConfiguration();

        public FormsMvpConfiguration Configuration { get { return _configuration; } }

        public void Run()
        {
            FormsPlatformServices.Current = _configuration.CreateFormsPlatformServices();
        }
    }
}
