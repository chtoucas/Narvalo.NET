namespace Playground.WebForms.Views
{
    using System.Collections.Generic;
    using Playground.WebForms.Services;

    public class EditWidgetModel
    {
        public int TotalCount { get; set; }

        public IEnumerable<Widget> Widgets { get; set; }
    }
}