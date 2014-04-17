// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System;

    public interface IPresenterFactory
    {
        IPresenter Create(Type presenterType, Type viewType, IView view);

        void Release(IPresenter presenter);
    }
}
