// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Controls
{
    using System;
    using System.Collections.Generic;

    using MvpWebForms.Entities;
    using MvpWebForms.Views;
    using Narvalo.Mvp.Web;

    public partial class WidgetsReadWriteControl
        : MvpUserControl<WidgetsReadWriteModel>, IWidgetsReadWriteView
    {
        public WidgetsReadWriteControl()
        {
            AutoDataBind = false;
        }

        public event EventHandler CountingWidgets;

        public event EventHandler<WidgetIdEventArgs> DeletingWidget;

        public event EventHandler<GettingWidgetsEventArgs> GettingWidgets;

        public event EventHandler<WidgetEventArgs> InsertingWidget;

        public event EventHandler<WidgetEventArgs> UpdatingWidget;

        public IEnumerable<Widget> GetWidgets(int maximumRows, int startRowIndex)
        {
            OnGettingWidgets(maximumRows, startRowIndex);
            return Model.Widgets;
        }

        public int CountWidgets()
        {
            OnCountingWidgets();
            return Model.Count;
        }

        public void UpdateWidget(Widget widget, Widget originalWidget)
        {
            OnUpdatingWidget(widget);
        }

        public void InsertWidget(Widget widget)
        {
            OnInsertingWidget(widget);
        }

        public void DeleteWidget(Widget widget)
        {
            OnDeletingWidget(widget);
        }

        private void OnGettingWidgets(int maximumRows, int startRowIndex)
        {
            if (GettingWidgets != null)
            {
                GettingWidgets(this, new GettingWidgetsEventArgs(maximumRows, startRowIndex));
            }
        }

        private void OnCountingWidgets()
        {
            if (CountingWidgets != null)
            {
                CountingWidgets(this, EventArgs.Empty);
            }
        }

        private void OnUpdatingWidget(Widget widget)
        {
            if (UpdatingWidget != null)
            {
                UpdatingWidget(this, new WidgetEventArgs(widget));
            }
        }

        private void OnInsertingWidget(Widget widget)
        {
            if (InsertingWidget != null)
            {
                InsertingWidget(this, new WidgetEventArgs(widget));
            }
        }

        private void OnDeletingWidget(Widget widget)
        {
            if (DeletingWidget != null)
            {
                DeletingWidget(this, new WidgetIdEventArgs(widget.Id));
            }
        }
    }
}
