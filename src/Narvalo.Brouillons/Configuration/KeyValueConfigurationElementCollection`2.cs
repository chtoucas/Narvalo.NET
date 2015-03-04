// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    using System.Configuration;
    using Narvalo;

    public class KeyValueConfigurationElementCollection<TKey, TValue>
    : ConfigurationElementCollection<KeyValuePairConfigurationElement<TKey, TValue>>
    {
        public TValue this[TKey key]
        {
            get { return (BaseGet(key) as KeyValuePairConfigurationElement<TKey, TValue>).Value; }
        }

        protected TKey GetKeyForItem(KeyValuePairConfigurationElement<TKey, TValue> item)
        {
            Require.NotNull(item, "item");

            return item.Key;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            Require.NotNull(element, "element");

            return GetKeyForItem(element as KeyValuePairConfigurationElement<TKey, TValue>);
        }
    }
}
