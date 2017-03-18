// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.Resources;

    using Xunit;

    public abstract class DefaultLocalizationFacts
    {
        private readonly Lazy<HashSet<string>> _defaultKeys;
        private readonly LocalizedStrings _localizedStrings;

        protected DefaultLocalizationFacts(ResourceManager resourceManager)
        {
            _localizedStrings = new LocalizedStrings(resourceManager);
            _defaultKeys = new Lazy<HashSet<string>>(() => _localizedStrings.GetKeys());
        }

        protected HashSet<string> DefaultKeys => _defaultKeys.Value;

        [Fact, UseCulture(LocalizedStrings.DefaultNeutralResourcesLanguage)]
        public void Neutral()
        {
            var dict = _localizedStrings.GetNeutralStrings();

            foreach (var pair in dict)
            {
                Assert.True(!String.IsNullOrWhiteSpace(pair.Value),
                    $"The resource '{pair.Key}' is empty or contains only white spaces.");
            }
        }

        [Fact, UseCulture("fr")]
        public void Français() => TestLanguage(tryParents: false);

        [Fact, UseCulture("vn")]
        public void TiếngViệt_IsMissing()
        {
            var dict = _localizedStrings.GetCurrentStrings(false);

            Assert.Null(dict);
        }

        private void TestLanguage(bool tryParents)
        {
            var dict = _localizedStrings.GetCurrentStrings(tryParents);

            Assert.NotNull(dict);

            foreach (var pair in dict)
            {
                Assert.True(DefaultKeys.Contains(pair.Key),
                    $"The resource contains an unrecognized key '{pair.Key}'.");
                Assert.True(!String.IsNullOrWhiteSpace(pair.Value),
                    $"The resource '{pair.Key}' is empty or contains only white spaces.");
            }

            foreach (var key in DefaultKeys)
            {
                Assert.True(dict.ContainsKey(key), $"The resource does not localize '{key}'.");
            }
        }
    }
}