namespace Narvalo.Collections
{
    public class ForEachItem<T>
    {
        /// <summary>
        /// Provides access to the original data item.
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// True if it is the first item in the list.
        /// </summary>
        public bool First { get; set; }

        /// <summary>
        /// True if it is the last item in the list.
        /// </summary>
        public bool Last { get; set; }

        /// <summary>
        /// Index of the item in the list.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// This bool represents if this item is the first in a new group.
        /// </summary>
        public bool NewGroup { get; set; }
    }
}
