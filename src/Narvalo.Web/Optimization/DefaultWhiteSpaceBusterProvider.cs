// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    public sealed partial class DefaultWhiteSpaceBusterProvider : IWhiteSpaceBusterProvider
    {
        private readonly IWhiteSpaceBuster _buster = new DefaultWhiteSpaceBuster();

        public IWhiteSpaceBuster Buster => _buster;
    }
}
