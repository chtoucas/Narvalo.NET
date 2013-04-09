namespace Narvalo.Configuration
{
    using System.Configuration;

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
            Requires.NotNull(item, "item");

            return item.Key;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            Requires.NotNull(element, "element");

            return GetKeyForItem(element as TItem);
        }
    }
}
