// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    public enum PresenterBindingMode
    {
        Default,

        /// <remarks>
        /// WARNING: This mode can only be used if the underlying view interface
        /// only defines properties or events. Also only applies to views that 
        /// use the "AttributeBasedPresenterDiscoveryStrategy" statregy.
        /// </remarks>
        SharedPresenter
    }
}
