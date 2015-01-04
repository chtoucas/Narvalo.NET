// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms
{
    using MvpWebForms.Presenters;
    using MvpWebForms.Views;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;

    [PresenterBinding(typeof(HelloWorldPresenter))]
    public partial class PageViewPage : MvpPage<StringModel> { }
}
