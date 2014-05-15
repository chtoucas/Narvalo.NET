// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

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