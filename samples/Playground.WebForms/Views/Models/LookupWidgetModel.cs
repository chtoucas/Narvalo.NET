namespace Playground.WebForms.Views.Models
{
    using System.Collections.Generic;
    using Playground.WebForms.Domain;

    public class LookupWidgetModel
    {
        public bool ShowResults { get; set; }
        public IList<Widget> Widgets { get; set; }
    }
}