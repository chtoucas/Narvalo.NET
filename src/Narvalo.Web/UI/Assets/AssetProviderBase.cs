// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI.Assets 
{
    using System;
    using System.Configuration.Provider;

    public abstract class AssetProviderBase : ProviderBase 
    {
        protected AssetProviderBase() { }

        public abstract Uri GetImage(string relativePath);

        public abstract Uri GetScript(string relativePath);

        public abstract Uri GetStyle(string relativePath);
    }
}
