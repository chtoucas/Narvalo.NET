// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Resources;

    public sealed class LocalizedStrings
    {
        // English is the default language used for resources.
        public const string DefaultNeutralResourcesLanguage = "en";

        private readonly ResourceManager _manager;
        private readonly Lazy<ResourceSet> _neutralResources;

        public LocalizedStrings(ResourceManager manager)
            : this(manager, DefaultNeutralResourcesLanguage) { }

        // lang = Neutral Resources Language.
        public LocalizedStrings(ResourceManager manager, string lang)
        {
            if (lang == null) { throw new ArgumentNullException(nameof(lang)); }

            _manager = manager ?? throw new ArgumentNullException(nameof(manager));

            // NB: We use tryParents = false, to be sure that we do not load any resource fallback.
            // Could we use GetNeutralResourcesLanguage(Assembly)?
            _neutralResources = new Lazy<ResourceSet>(
                () => _manager.GetResourceSet(new CultureInfo(lang), true, tryParents: false));
        }

        private ResourceSet NeutralResources => _neutralResources.Value ?? throw new NotSupportedException();

        public HashSet<string> GetKeys()
        {
            var retval = new HashSet<string>();

            foreach (DictionaryEntry item in NeutralResources)
            {
                // We only keep resources that are of type string.
                if (!(item.Value is string)) { continue; }
                retval.Add(item.Key.ToString());
            }

            return retval;
        }

        public Dictionary<string, string> GetNeutralStrings()
            => ToDictionary(NeutralResources);

        public Dictionary<string, string> GetCurrentStrings(bool tryParents)
        {
            var set = _manager.GetResourceSet(CultureInfo.CurrentCulture, true, tryParents);

            if (set == null) { return null; }

            return ToDictionary(set);
        }

        private static Dictionary<string, string> ToDictionary(ResourceSet resources)
            => resources.Cast<DictionaryEntry>()
                .Where(item => item.Value is string)
                .ToDictionary(item => item.Key.ToString(),
                              item => item.Value.ToString());
    }
}
