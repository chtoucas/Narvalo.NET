// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    public sealed class NoopWhiteSpaceBuster : IWhiteSpaceBuster
    {
        public string Bust(string value)
        {
            return value;
        }
    }
}
