// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;

    public abstract class ConfigurationElementCollection<TElement>
        : BasicConfigurationElementCollection<TElement>, IList<TElement>
        where TElement : ConfigurationElement
    {
        public new bool IsReadOnly
        {
            get { return base.IsReadOnly(); }
        }

        public void Add(TElement item)
        {
            Require.NotNull(item, "item");

            BaseAdd(item, true /* throwIfExists */);
        }

        public void Clear()
        {
            BaseClear();
        }

        public bool Contains(TElement item)
        {
            Require.NotNull(item, "item");

            return BaseIndexOf(item) >= 0;
        }

        public void CopyTo(TElement[] array, int arrayIndex)
        {
            Require.NotNull(array, "array");

            base.CopyTo(array, arrayIndex);
        }

        public bool Remove(TElement item)
        {
            Require.NotNull(item, "item");

            int index = BaseIndexOf(item);
            if (index >= 0)
            {
                BaseRemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public TElement this[int index]
        {
            get { return BaseGet(index) as TElement; }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public int IndexOf(TElement item)
        {
            Require.NotNull(item, "item");

            return BaseIndexOf(item);
        }

        public void Insert(int index, TElement item)
        {
            Require.NotNull(item, "item");

            BaseAdd(index, item);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
    }
}
