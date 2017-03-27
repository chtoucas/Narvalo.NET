// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo {
    using System.Resources;

    using Assert = Narvalo.AssertExtended;

    public abstract class DefaultLocalizationFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base("L10N", description) { }
        }

        protected DefaultLocalizationFacts(ResourceManager manager)
            => LocalizedStrings = new LocalizedStrings(manager);

        protected LocalizedStrings LocalizedStrings { get; }

        [t("English localization is available and looks good."), UseCulture("en")]
        public void english_is_supported() => Assert.IsLocalized(LocalizedStrings);

        [t("La traduction des messages en français est disponible."), UseCulture("fr")]
        public void français_is_supported() => Assert.IsLocalized(LocalizedStrings);

        [t("Le français est pleinement supporté."), UseCulture("fr")]
        public void français_is_complete() => Assert.IsLocalizationComplete(LocalizedStrings);

        [t("Tiếng Việt localization is not available."), UseCulture("vn")]
        public void tiếng_việt_is_not_supported() => Assert.IsNotLocalized(LocalizedStrings);
    }
}
