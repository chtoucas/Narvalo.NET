namespace Playground.Views
{
    using System.Collections.Generic;
    using Playground.Data;

    public class EditWidgetModel
    {
        public int WidgetCount { get; set; }

        public IEnumerable<Widget> Widgets { get; set; }
    }
}