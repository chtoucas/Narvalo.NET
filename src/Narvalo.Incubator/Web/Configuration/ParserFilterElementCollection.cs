namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;
    using Narvalo.Configuration;

    public class ParserFilterElementCollection
        : KeyedConfigurationElementCollection<Type, ParserFilterElement>
    {
        public ParserFilterElementCollection() : base() { }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMapAlternate; }
        }
    }
}
