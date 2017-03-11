// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;

    public partial interface IPresenterFactory
    {
        IPresenter Create(Type presenterType, Type viewType, IView view);

        void Release(IPresenter presenter);
    }
}
