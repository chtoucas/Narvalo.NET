namespace Playground.Views
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Mvp;
    using Playground.Data;

    public interface ILookupWidgetView : IView<LookupWidgetModel>
    {
        event EventHandler<WidgetIdEventArgs> Finding;
    }

    public sealed class LookupWidgetModel
    {
        readonly IList<Widget> _widgets = new List<Widget>();

        public bool ShowResults { get; set; }

        public IList<Widget> Widgets { get { return _widgets; } }
    }

    public sealed class WidgetIdEventArgs : EventArgs
    {
        public WidgetIdEventArgs(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }
}