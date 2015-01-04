// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Views
{
    using System.Collections.Generic;
    using MvpWebForms.Entities;

    public sealed class WidgetsReadWriteModel
    {
        public int Count { get; set; }

        public IEnumerable<Widget> Widgets { get; set; }
    }
}
