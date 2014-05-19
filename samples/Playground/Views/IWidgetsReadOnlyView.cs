namespace Playground.Views
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Mvp;
    using Playground.Model;

    public interface IWidgetsReadOnlyView : IView<WidgetsReadOnlyModel>
    {
        event EventHandler<WidgetIdEventArgs> Finding;

        event EventHandler<WidgetIdEventArgs> FindingApm;

        event EventHandler<WidgetIdEventArgs> FindingTap;
    }

    public sealed class WidgetsReadOnlyModel
    {
        readonly IList<Widget> _widgets = new List<Widget>();

        public bool ShowResult { get; set; }

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