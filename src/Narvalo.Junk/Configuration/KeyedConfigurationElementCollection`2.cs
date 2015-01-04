// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    using System.Configuration;
    using Narvalo;

    public class KeyedConfigurationElementCollection<TKey, TItem>
        : ConfigurationElementCollection<TItem>
        where TItem : ConfigurationElement, IKeyedConfigurationElement<TKey>
    {
        public TItem this[TKey key]
        {
            get { return BaseGet(key) as TItem; }
        }

        protected TKey GetKeyForItem(TItem item)
        {
            Require.NotNull(item, "item");

            return item.Key;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            Require.NotNull(element, "element");

            return GetKeyForItem(element as TItem);
        }
    }
}
