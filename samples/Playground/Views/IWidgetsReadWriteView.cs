namespace Playground.Views
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Mvp;
    using Playground.Model;

    public interface IWidgetsReadWriteView : IView<WidgetsReadWriteModel>
    {
        event EventHandler CountingWidgets;
        event EventHandler<WidgetIdEventArgs> DeletingWidget;
        event EventHandler<GettingWidgetsEventArgs> GettingWidgets;
        event EventHandler<WidgetEventArgs> InsertingWidget;
        event EventHandler<WidgetEventArgs> UpdatingWidget;
    }

    public sealed class WidgetsReadWriteModel
    {
        public int Count { get; set; }

        public IEnumerable<Widget> Widgets { get; set; }
    }

    public sealed class GettingWidgetsEventArgs : EventArgs
    {
        public GettingWidgetsEventArgs(int maximumRows, int startRowIndex)
        {
            MaximumRows = maximumRows;
            StartRowIndex = startRowIndex;
        }

        public int MaximumRows { get; private set; }
        public int StartRowIndex { get; private set; }
    }

    public sealed class WidgetEventArgs : EventArgs
    {
        public WidgetEventArgs(Widget widget)
        {
            Widget = widget;
        }

        public Widget Widget { get; private set; }
    }
}