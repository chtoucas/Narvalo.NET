namespace Playground.Views
{
    using System.Collections.Generic;
    using Playground.Services;

    public class EditWidgetModel
    {
        public int WidgetCount { get; set; }

        public IEnumerable<Widget> Widgets { get; set; }
    }
}