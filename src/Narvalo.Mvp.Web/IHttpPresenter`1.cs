// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    public interface IHttpPresenter<out TView> 
        : IPresenter<TView>, IHttpPresenter where TView : IView { }
}
