// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.Resources;

    using Xunit;

    using Assert = Narvalo.AssertExtended;

    public abstract class DefaultLocalizationFacts
    {
        private readonly HashSet<string> _defaultKeys;
        private readonly ResourceManager _resourceManager;

        protected DefaultLocalizationFacts(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
            _defaultKeys = StringsResource.GetAllKeys(_resourceManager);
        }

        [Fact, UseEnglish]
        public void TestEnglish() => Assert.IsLocalized(_resourceManager);

        [Fact, UseFrançais]
        public void TestFrench() => Assert.IsLocalized(_resourceManager, _defaultKeys);

        [Fact, UseItaliano]
        public void TestItalian() => Assert.IsNotLocalized(_resourceManager);
    }
}