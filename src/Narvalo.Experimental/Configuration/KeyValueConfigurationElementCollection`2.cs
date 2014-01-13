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
            Requires.NotNull(item, "item");

            return item.Key;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            Requires.NotNull(element, "element");

            return GetKeyForItem(element as KeyValuePairConfigurationElement<TKey, TValue>);
        }
    }
}
