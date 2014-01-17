namespace Narvalo.Collections
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Fournit des méthodes d'extension pour <see cref="System.Collections.Generic.IEnumerator{T}"/>.
    /// </summary>
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
