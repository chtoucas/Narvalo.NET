namespace Playground.WebForms.Views.Models
{
    using System.Collections.Generic;
    using Playground.WebForms.Domain;

    public class EditWidgetModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<Widget> Widgets { get; set; }
    }
}