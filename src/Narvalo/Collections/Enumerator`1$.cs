namespace Narvalo.Collections
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public static class EnumeratorExtensions
    {
        public static Collection<T> ToCollection<T>(this IEnumerator<T> source)
        {
            Requires.Object(source);

            var result = new Collection<T>();

            while (source.MoveNext()) {
                result.Add(source.Current);
            }

            return result;
        }
    }
}
