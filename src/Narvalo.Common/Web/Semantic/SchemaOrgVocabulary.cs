namespace Narvalo.Web.Semantic
{
    using Narvalo;

    public class SchemaOrgVocabulary
    {
        string _itemType = SchemaOrgType.WebPage;

        public string ItemType
        {
            get { return _itemType; }
            set
            {
                Requires.NotNullOrEmpty(value, "value");
                _itemType = value;
            }
        }
    }
}