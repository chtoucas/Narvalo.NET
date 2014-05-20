namespace Playground.Views
{
    using System;
    using Playground.Entities;

    public sealed class WidgetEventArgs : EventArgs
    {
        public WidgetEventArgs(Widget widget)
        {
            Widget = widget;
        }

        public Widget Widget { get; private set; }
    }
}