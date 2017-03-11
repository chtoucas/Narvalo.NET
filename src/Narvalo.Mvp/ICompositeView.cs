// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    public partial interface ICompositeView : IView
    {
        void Add(IView view);
    }
}
