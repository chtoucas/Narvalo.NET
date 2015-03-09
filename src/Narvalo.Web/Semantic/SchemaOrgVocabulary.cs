// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Semantic
{
    public sealed class SchemaOrgVocabulary
    {
        private string _itemType = SchemaOrgType.WebPage;

        public string ItemType
        {
            get
            {
                return _itemType;
            }

            set
            {
                Require.PropertyNotEmpty(value);

                _itemType = value;
            }
        }
    }
}