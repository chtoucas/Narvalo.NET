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