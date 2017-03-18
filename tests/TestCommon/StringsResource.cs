// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Resources;

    public class StringsResource
    {
        // English is the neutral language we use for resources.
        private static readonly CultureInfo s_EnglishCulture = new CultureInfo("en");

        public static HashSet<string> GetAllKeys(ResourceManager resourceManager)
            => GetAllKeys(resourceManager, s_EnglishCulture);

        public static HashSet<string> GetAllKeys(ResourceManager resourceManager, CultureInfo cultureInfo)
        {
            if (resourceManager == null) { throw new ArgumentNullException(nameof(resourceManager)); }

            ResourceSet set = resourceManager.GetResourceSet(cultureInfo, true, false);

            var retval = new HashSet<string>();

            foreach (DictionaryEntry item in set)
            {
                // We only keep resources that are of type string.
                if (!(item.Value is string)) { continue; }
                retval.Add(item.Key.ToString());
            }

            return retval;
        }
    }
}
