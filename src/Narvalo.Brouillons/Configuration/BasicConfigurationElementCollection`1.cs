// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;

    public abstract class BasicConfigurationElementCollection<TElement>
        : ConfigurationElementCollection, IEnumerable<TElement>
        where TElement : ConfigurationElement
    {
        public new IEnumerator<TElement> GetEnumerator()
        {
            foreach (TElement element in (IEnumerable)this)
            {
                yield return element;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            // NB: On n'utilise pas la contrainte générique new() car cette
            // méthode peut-être surchargée dans une classe dérivée.
            return Activator.CreateInstance<TElement>();
        }
    }
}

