// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Controls
{
    using MvpWebForms.Presenters;
    using MvpWebForms.Views;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;

    [PresenterBinding(
        typeof(SharedPresenter),
        ViewType = typeof(IView<StringModel>),
        BindingMode = PresenterBindingMode.SharedPresenter)]
    public partial class SharedPresenterControl1 : MvpUserControl<StringModel> { }
}
