// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    public interface IPresenter<out TView> : IPresenter where TView : IView
    {
        TView View { get; }
    }
}
