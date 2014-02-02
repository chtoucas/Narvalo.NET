namespace Narvalo.Web.Semantic
{
    using Narvalo;

    public sealed class SchemaOrgVocabulary
    {
        string _itemType = SchemaOrgType.WebPage;

        public string ItemType
        {
            get { return _itemType; }
            set { _itemType = Require.PropertyNotEmpty(value); }
        }
    }
}