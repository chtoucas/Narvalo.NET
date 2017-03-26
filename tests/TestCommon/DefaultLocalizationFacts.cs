// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo {
    using System.Resources;

    using Assert = Narvalo.AssertExtended;

    public abstract class DefaultLocalizationFacts {
        internal sealed class factAttribute : FactAttribute_ {
            public factAttribute(string message) : base("L10N", message) { }
        }

        protected DefaultLocalizationFacts(ResourceManager manager)
            => LocalizedStrings = new LocalizedStrings(manager);

        protected LocalizedStrings LocalizedStrings { get; }

        [fact("English localization is available and looks good."), UseCulture("en")]
        public void english_is_supported() => Assert.IsLocalized(LocalizedStrings);

        [fact("La traduction des messages en français est disponible."), UseCulture("fr")]
        public void français_is_supported() => Assert.IsLocalized(LocalizedStrings);

        [fact("Le français est pleinement supporté."), UseCulture("fr")]
        public void français_is_complete() => Assert.IsLocalizationComplete(LocalizedStrings);

        [fact("Tiếng Việt localization is not available."), UseCulture("vn")]
        public void tiếng_việt_is_not_supported() => Assert.IsNotLocalized(LocalizedStrings);
    }
}
