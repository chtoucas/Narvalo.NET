// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Views
{
    using System;

    using MvpWebForms.Entities;

    public sealed class WidgetEventArgs : EventArgs
    {
        public WidgetEventArgs(Widget widget)
        {
            Widget = widget;
        }

        public Widget Widget { get; private set; }
    }
}
