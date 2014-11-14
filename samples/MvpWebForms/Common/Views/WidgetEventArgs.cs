namespace MvpWebForms.Views
{
    using System;
    using MvpWebForms.Entities;

    public sealed class WidgetEventArgs : EventArgs
    {
        public WidgetEventArgs(Widget widget)
        {
            Widget = widget;
        }

        public Widget Widget { get; private set; }
    }
}