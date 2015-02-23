// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Views
{
    using System.Collections.Generic;

    using MvpWebForms.Entities;

    public sealed class WidgetsReadOnlyModel
    {
        private readonly IList<Widget> _widgets = new List<Widget>();

        public bool ShowResult { get; set; }

        public IList<Widget> Widgets { get { return _widgets; } }
    }
}
