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
        #region IEnumerable<TElement>

        public new IEnumerator<TElement> GetEnumerator()
        {
            foreach (TElement element in (IEnumerable)this) {
                yield return element;
            }
        }

        #endregion
        
        protected override ConfigurationElement CreateNewElement()
        {
            // NB: on n'utilise pas la contrainte générique new() car cette
            // méthode peut-être surchargée dans une classe dérivée.
            return Activator.CreateInstance<TElement>();
        }
    }
}

