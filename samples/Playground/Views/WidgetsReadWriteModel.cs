namespace Playground.Views
{
    using System.Collections.Generic;
    using Playground.Entities;

    public sealed class WidgetsReadWriteModel
    {
        public int Count { get; set; }

        public IEnumerable<Widget> Widgets { get; set; }
    }
}