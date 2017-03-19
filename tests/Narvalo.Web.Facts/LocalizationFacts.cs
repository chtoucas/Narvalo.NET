// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#if !NO_INTERNALS_VISIBLE_TO

namespace Narvalo.Web
{
    using Narvalo.Web.Properties;

    public class LocalizationFacts : DefaultLocalizationFacts
    {
        public LocalizationFacts() : base(Strings.ResourceManager) { }
    }
}

#endif
