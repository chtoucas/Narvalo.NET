// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Resources;

    using Xunit;

    using Assert = Narvalo.AssertExtended;

    public abstract class DefaultLocalizationFacts
    {
        protected DefaultLocalizationFacts(ResourceManager manager)
            => LocalizedStrings = new LocalizedStrings(manager);

        protected LocalizedStrings LocalizedStrings { get; }

        [Fact, UseCulture("en")]
        public void English_IsSupported() => Assert.IsLocalized(LocalizedStrings);

        [Fact, UseCulture("fr")]
        public void Français_IsSupported() => Assert.IsLocalized(LocalizedStrings);

        [Fact, UseCulture("fr")]
        public void Français_IsComplete() => Assert.IsLocalizationComplete(LocalizedStrings);

        [Fact, UseCulture("vn")]
        public void TiếngViệt_IsNotSupported() => Assert.IsNotLocalized(LocalizedStrings);
    }
}
