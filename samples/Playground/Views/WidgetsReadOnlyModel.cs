namespace Playground.Views
{
    using System.Collections.Generic;
    using Playground.Entities;

    public sealed class WidgetsReadOnlyModel
    {
        readonly IList<Widget> _widgets = new List<Widget>();

        public bool ShowResult { get; set; }

        public IList<Widget> Widgets { get { return _widgets; } }
    }
}