// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Resources;

    public partial class AssertExtended : Xunit.Assert
    {
        public static void NotNullOrWhiteSpace(string value)
            => True(!String.IsNullOrWhiteSpace(value));

        public static void NotNullOrWhiteSpace(string value, string userMessage)
            => True(!String.IsNullOrWhiteSpace(value), userMessage);

        public static void IsNotLocalized(ResourceManager resourceManager)
        {
            var res = resourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, false);
            Null(res);
        }

        public static void IsLocalized(ResourceManager resourceManager)
        {
            ResourceSet set = resourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, false);
            NotNull(set);

            var dict = set.Cast<DictionaryEntry>()
                .Where(item => item.Value is string)
                .ToDictionary(item => item.Key.ToString(),
                              item => item.Value.ToString());

            foreach (var pair in dict)
            {
                True(!String.IsNullOrWhiteSpace(pair.Value), $"The resource '{pair.Key}' is empty or contains only white spaces.");
            }
        }

        public static void IsLocalized(ResourceManager resourceManager, HashSet<string> defaultKeys)
        {
            ResourceSet set = resourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, false);
            NotNull(set);

            var dict = set.Cast<DictionaryEntry>()
                .Where(item => item.Value is string)
                .ToDictionary(item => item.Key.ToString(),
                              item => item.Value.ToString());

            foreach (var pair in dict)
            {
                True(defaultKeys.Contains(pair.Key), $"The resource contains an unrecognized key '{pair.Key}'.");
                True(!String.IsNullOrWhiteSpace(pair.Value), $"The resource '{pair.Key}' is empty or contains only white spaces.");
            }

            foreach (var key in defaultKeys)
            {
                True(dict.ContainsKey(key), $"The resource does not localize '{key}'.");
            }
        }
    }
}
