// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using Narvalo.Mvp.Properties;
    using Xunit;

    using Assert = Narvalo.AssertExtended;

    public static class LocalizationFacts
    {
        [Fact, UseCulture("en")]
        public static void English_IsLocalized() => Assert.IsLocalized(Strings.ResourceManager);
    }
}