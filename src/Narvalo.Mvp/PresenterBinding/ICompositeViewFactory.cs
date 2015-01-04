// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Collections.Generic;

    public interface ICompositeViewFactory
    {
        ICompositeView Create(Type viewType, IEnumerable<IView> views);
    }
}
