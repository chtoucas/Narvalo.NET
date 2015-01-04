// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Views
{
    using System;
    using Narvalo.Mvp;

    public interface IWidgetsReadWriteView : IView<WidgetsReadWriteModel>
    {
        event EventHandler CountingWidgets;

        event EventHandler<WidgetIdEventArgs> DeletingWidget;

        event EventHandler<GettingWidgetsEventArgs> GettingWidgets;

        event EventHandler<WidgetEventArgs> InsertingWidget;

        event EventHandler<WidgetEventArgs> UpdatingWidget;
    }
}
