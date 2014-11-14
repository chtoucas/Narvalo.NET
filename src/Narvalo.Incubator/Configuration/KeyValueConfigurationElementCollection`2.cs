namespace Narvalo.Configuration
{
    using System.Configuration;
    using Narvalo.Diagnostics;

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
