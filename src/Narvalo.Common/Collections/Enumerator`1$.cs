namespace Narvalo.Collections
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public static class EnumeratorExtensions
    {
        public static Collection<T> ToCollection<T>(this IEnumerator<T> @this)
        {
            Requires.Object(@this);

            var result = new Collection<T>();

            while (@this.MoveNext()) {
                result.Add(@this.Current);
            }

            return result;
        }
    }
}
