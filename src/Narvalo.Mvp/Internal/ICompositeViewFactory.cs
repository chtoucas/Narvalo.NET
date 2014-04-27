// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using Narvalo.Mvp.Binder;

    internal interface ICompositeViewFactory
    {
        IView Create(PresenterBinding binding);
    }
}
