namespace Playground.Views
{
    using System.Collections.Generic;
    using Playground.Data;

    public class LookupWidgetModel
    {
        readonly IList<Widget> _widgets = new List<Widget>();

        public bool ShowResults { get; set; }

        public IList<Widget> Widgets { get { return _widgets; } }
    }
}