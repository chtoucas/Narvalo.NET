namespace Playground.Controls
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Mvp.Web;
    using Playground.Services;
    using Playground.Views;

    public partial class EditWidgetControl : MvpUserControl<EditWidgetModel>, IEditWidgetView
    {
        public EditWidgetControl()
        {
            AutoDataBind = false;
        }

        public event EventHandler CountingWidgets;
        public event EventHandler<EditingWidgetEventArgs> DeletingWidget;
        public event EventHandler<GettingWidgetEventArgs> GettingWidgets;
        public event EventHandler<EditingWidgetEventArgs> InsertingWidget;
        public event EventHandler<UpdatingWidgetEventArgs> UpdatingWidget;

        public IEnumerable<Widget> GetWidgets(int maximumRows, int startRowIndex)
        {
            OnGettingWidgets(maximumRows, startRowIndex);
            return Model.Widgets;
        }

        public int CountWidgets()
        {
            OnCountingWidgets();
            return Model.WidgetCount;
        }

        public void UpdateWidget(Widget widget, Widget originalWidget)
        {
            OnUpdatingWidget(widget, originalWidget);
        }

        public void InsertWidget(Widget widget)
        {
            OnInsertingWidget(widget);
        }

        public void DeleteWidget(Widget widget)
        {
            OnDeletingWidget(widget);
        }

        void OnGettingWidgets(int maximumRows, int startRowIndex)
        {
            if (GettingWidgets != null) {
                GettingWidgets(this, new GettingWidgetEventArgs(maximumRows, startRowIndex));
            }
        }

        void OnCountingWidgets()
        {
            if (CountingWidgets != null) {
                CountingWidgets(this, EventArgs.Empty);
            }
        }

        void OnUpdatingWidget(Widget widget, Widget originalWidget)
        {
            if (UpdatingWidget != null) {
                UpdatingWidget(this, new UpdatingWidgetEventArgs(widget, originalWidget));
            }
        }

        void OnInsertingWidget(Widget widget)
        {
            if (InsertingWidget != null) {
                InsertingWidget(this, new EditingWidgetEventArgs(widget));
            }
        }

        void OnDeletingWidget(Widget widget)
        {
            if (DeletingWidget != null) {
                DeletingWidget(this, new EditingWidgetEventArgs(widget));
            }
        }
    }
}