// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Stubs
{
    using System;

    public interface ISimpleViewWithModel : IView<ViewModel>
    {
        event EventHandler TestHandler;
    }
}
