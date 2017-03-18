// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Resources;

    public partial class AssertExtended : Xunit.Assert
    {
        public static void IsLocalized(ResourceManager manager)
        {
            var dict = new LocalizedStrings(manager).GetNeutralStrings();

            foreach (var pair in dict)
            {
                True(!String.IsNullOrWhiteSpace(pair.Value),
                    $"The resource '{pair.Key}' is empty or contains only white spaces.");
            }
        }
    }
}
