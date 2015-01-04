// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Views
{
    using System;

    public sealed class GettingWidgetsEventArgs : EventArgs
    {
        public GettingWidgetsEventArgs(int maximumRows, int startRowIndex)
        {
            MaximumRows = maximumRows;
            StartRowIndex = startRowIndex;
        }

        public int MaximumRows { get; private set; }

        public int StartRowIndex { get; private set; }
    }
}
