namespace Playground.Views
{
    using System;
    using Narvalo.Mvp;
    using Playground.Data;

    public interface IEditWidgetView : IView<EditWidgetModel>
    {
        event EventHandler CountingWidgets;
        event EventHandler<EditingWidgetEventArgs> DeletingWidget;
        event EventHandler<GettingWidgetEventArgs> GettingWidgets;
        event EventHandler<EditingWidgetEventArgs> InsertingWidget;
        event EventHandler<UpdatingWidgetEventArgs> UpdatingWidget;
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

    public class UpdatingWidgetEventArgs : EventArgs
    {
        public Widget Widget { get; private set; }
        public Widget OriginalWidget { get; private set; }

        public UpdatingWidgetEventArgs(Widget widget, Widget originalWidget)
        {
            Widget = widget;
            OriginalWidget = originalWidget;
        }
    }

    public class EditingWidgetEventArgs : EventArgs
    {
        public Widget Widget { get; private set; }

        public EditingWidgetEventArgs(Widget widget)
        {
            Widget = widget;
        }
    }
}