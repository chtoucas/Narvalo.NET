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

        [Fact(DisplayName = "English localization is available and looks good."), UseCulture("en")]
        public void English_IsSupported() => Assert.IsLocalized(LocalizedStrings);

        [Fact(DisplayName = "La traduction des messages en français est disponible."), UseCulture("fr")]
        public void Français_IsSupported() => Assert.IsLocalized(LocalizedStrings);

        [Fact(DisplayName = "Le français est pleinement supporté."), UseCulture("fr")]
        public void Français_IsComplete() => Assert.IsLocalizationComplete(LocalizedStrings);

        [Fact(DisplayName = "Tiếng Việt localization is not available."), UseCulture("vn")]
        public void TiếngViệt_IsNotSupported() => Assert.IsNotLocalized(LocalizedStrings);
    }
}
