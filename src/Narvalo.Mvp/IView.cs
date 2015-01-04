// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;

    public interface IView
    {
        event EventHandler Load;
        
        bool ThrowIfNoPresenterBound { get; }
    }
}
