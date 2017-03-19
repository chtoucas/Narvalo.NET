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
        private readonly ResourceManager _manager;
        private readonly CultureInfo _referenceCulture;
        private readonly Lazy<HashSet<string>> _referenceKeys;

        // REVIEW: Shouldn't we use the assembly attribute NeutralResourcesLanguage
        // to determine the default culture.
        public LocalizedStrings(ResourceManager manager)
            : this(manager, CultureInfo.InvariantCulture) { }

        public LocalizedStrings(ResourceManager manager, string referenceCulture)
            : this(manager, new CultureInfo(referenceCulture)) { }

        // defaultCulture = the culture we will use as a baseline.
        public LocalizedStrings(ResourceManager manager, CultureInfo referenceCulture)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _referenceCulture = referenceCulture ?? throw new ArgumentNullException(nameof(referenceCulture));

            _referenceKeys = new Lazy<HashSet<string>>(GetReferenceKeys);
        }

        public HashSet<string> ReferenceKeys => _referenceKeys.Value;

        public Dictionary<string, string> GetStrings()
            => GetStrings(CultureInfo.CurrentCulture, false);

        public Dictionary<string, string> GetStrings(bool tryParents)
            => GetStrings(CultureInfo.CurrentCulture, tryParents);

        public Dictionary<string, string> GetStrings(CultureInfo cultureInfo, bool tryParents)
        {
            var set = GetResourceSet(cultureInfo, tryParents);

            if (set == null) { return null; }

            return set.Cast<DictionaryEntry>()
                .Where(item => item.Value is string)
                .ToDictionary(item => item.Key.ToString(),
                              item => item.Value.ToString());
        }

        private ResourceSet GetResourceSet(CultureInfo cultureInfo, bool tryParents)
            => _manager.GetResourceSet(cultureInfo, true, tryParents);

        private HashSet<string> GetReferenceKeys()
        {
            var set = GetResourceSet(_referenceCulture, false);

            if (set == null) { return null; }

            var retval = new HashSet<string>();

            foreach (DictionaryEntry item in set)
            {
                // We only keep resources that are of type string.
                if (!(item.Value is string)) { continue; }
                retval.Add(item.Key.ToString());
            }

            return retval;
        }
    }
}
