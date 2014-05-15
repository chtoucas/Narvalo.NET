﻿namespace Playground.WebForms.Views.Models
{
    using System.Collections.Generic;
    using Playground.WebForms.Services;

    public class LookupWidgetModel
    {
        readonly IList<Widget> _widgets = new List<Widget>();

        public bool ShowResults { get; set; }

        public IList<Widget> Widgets { get { return _widgets; } }
    }
}