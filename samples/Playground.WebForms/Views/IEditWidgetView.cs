namespace Playground.WebForms.Views
{
    using System;
    using Narvalo.Mvp;
    using Playground.WebForms.Domain;
    using Playground.WebForms.Views.Models;

    public interface IEditWidgetView : IView<EditWidgetModel>
    {
        event EventHandler<GettingWidgetEventArgs> GettingWidgets;
        event EventHandler GettingWidgetsTotalCount;
        event EventHandler<UpdateWidgetEventArgs> UpdatingWidget;
        event EventHandler<EditWidgetEventArgs> InsertingWidget;
        event EventHandler<EditWidgetEventArgs> DeletingWidget;
    }

    public class GettingWidgetEventArgs : EventArgs
    {
        public int MaximumRows { get; private set; }
        public int StartRowIndex { get; private set; }

        public GettingWidgetEventArgs(int maximumRows, int startRowIndex)
        {
            MaximumRows = maximumRows;
            StartRowIndex = startRowIndex;
        }
    }

    public class UpdateWidgetEventArgs : EventArgs
    {
        public Widget Widget { get; private set; }
        public Widget OriginalWidget { get; private set; }

        public UpdateWidgetEventArgs(Widget widget, Widget originalWidget)
        {
            Widget = widget;
            OriginalWidget = originalWidget;
        }
    }

    public class EditWidgetEventArgs : EventArgs
    {
        public Widget Widget { get; private set; }

        public EditWidgetEventArgs(Widget widget)
        {
            Widget = widget;
        }
    }
}