namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;

    public class NamedConfigurationElementCollection<TKey, TElement>
        : BasicConfigurationElementCollection<TElement>
        where TElement : ConfigurationElement, IKeyedConfigurationElement<TKey>
    {
        readonly string _elementName;

        protected NamedConfigurationElementCollection(string elementName)
            : base()
        {
            Requires.NotNullOrEmpty(elementName, "elementName");

            _elementName = elementName;
        }

        public TElement this[TKey key]
        {
            get { return BaseGet(key) as TElement; }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return _elementName; }
        }

        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return Activator.CreateInstance<TElement>();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            Requires.NotNull(element, "element");

            return GetElementKey(element as TElement);
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName != null && elementName == _elementName;
        }

        protected TKey GetElementKey(TElement element)
        {
            Requires.NotNull(element, "element");

            return element.Key;
        }
    }

    //protected override object GetElementKey(ConfigurationElement element)
    //{
    //    Requires.NotNull(element, "element");

    //    return (string)element.ElementInformation.Properties[_elementKey].Value;
    //}

    //    <configSections>  
    //  <section name="mySpecialSection" type="MyNamespace.MySpecialConfigurationSection, MyAssembly"/>   
    //</configSections>  

    //...  

    //<mySpecialSection>  
    // <items>  
    //  <apple value="one"/>  
    //  <apple value="two"/>  
    //  <orange value="one"/>  
    // </items>  
    //</mySpecialSection>  

    //public class MySpecialConfigurationSection : ConfigurationSection
    //{
    //    [ConfigurationProperty("items", IsRequired = false, IsKey = false, IsDefaultCollection = false)]
    //    public ItemCollection Items
    //    {
    //        get { return ((ItemCollection)(base["items"])); }
    //        set { base["items"] = value; }
    //    }
    //}

    //[ConfigurationCollection(typeof(Item), AddItemName = "apple,orange", CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
    //public class ItemCollection : ConfigurationElementCollection
    //{
    //    public override ConfigurationElementCollectionType CollectionType
    //    {
    //        get { return ConfigurationElementCollectionType.BasicMapAlternate; }
    //    }

    //    protected override string ElementName
    //    {
    //        get { return string.Empty; }
    //    }

    //    protected override bool IsElementName(string elementName)
    //    {
    //        return (elementName == "apple" || elementName == "orange");
    //    }

    //    protected override object GetElementKey(ConfigurationElement element)
    //    {
    //        return element;
    //    }

    //    protected override ConfigurationElement CreateNewElement()
    //    {
    //        return new Item();
    //    }

    //    protected override ConfigurationElement CreateNewElement(string elementName)
    //    {
    //        var item = new Item();
    //        if (elementName == "apple") {
    //            item.Type = ItemType.Apple;
    //        }
    //        else if (elementName == "orange") {
    //            item.Type = ItemType.Orange;
    //        }
    //        return item;
    //    }

    //    public override bool IsReadOnly()
    //    {
    //        return false;
    //    }
    //}

    //public enum ItemType
    //{
    //    Apple,
    //    Orange
    //}

    //public class Item
    //{
    //    public ItemType Type { get; set; }

    //    [ConfigurationProperty("value")]
    //    public string Value
    //    {
    //        get { return (string)base["value"]; }
    //        set { base["value"] = value; }
    //    }
    //}


}


