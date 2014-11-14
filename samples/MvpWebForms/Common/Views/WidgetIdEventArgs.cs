namespace MvpWebForms.Views
{
    using System;

    public sealed class WidgetIdEventArgs : EventArgs
    {
        public WidgetIdEventArgs(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }
}