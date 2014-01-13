namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Diagnostics;

    public static class EnumerableExtensions
    {
        public static IEnumerable<ForEachItem<T>> ToFor<T>(this IEnumerable<T> @this)
        {
            Requires.Object(@this);

            return ToFor(@this, 0);
        }

        /// <summary>
        /// </summary>
        /// See: http://www.paulwheeler.com/blog/asp-net-mvc-foreach-loop-sugar
        /// The second method allows you to provide an integer which will set NewGroup
        /// to true for every X items. This can be used to produce things like the
        /// RepeatColumns function of the DataList. Here is some example usage to create
        /// a table from a loop that has 4 columns across:
        /// <example>
        /// <table>
        ///    <tr>
        ///        <% foreach (var m in users.ToFor(4)) { %>
        ///            <td><%= m.Item.Name %></td>
        ///            <% if (m.NewGroup) app.Write("</tr><tr>"); %>
        ///        <% } %>
        ///    </tr>
        /// </table>
        /// </example>
        /// Another use could be to format a menu of links for a website that need
        /// to have a pipe character between the links:
        /// <example>
        /// <%
        ///    foreach (var p in pages.ToFor()) {
        ///        app.Write("<a href='{0}'>{1}</a>", p.Item.Href, p.Item.Title);
        ///        if (!p.Last) app.Write("&nbsp;&nbsp;|&nbsp;&nbsp;");
        ///    }
        /// %>
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static IEnumerable<ForEachItem<T>> ToFor<T>(this IEnumerable<T> @this, int group)
        {
            Requires.Object(@this);

            var list = new List<ForEachItem<T>>();
            var count = list.Count;

            foreach (var item in @this) {
                var fei = new ForEachItem<T> {
                    First = count == 0,
                    Item = item,
                    Index = count,
                    NewGroup = group > 0 && (count % group == group - 1)
                };
                list.Add(fei);
            }

            if (count > 0) {
                list[count - 1].Last = true;
            }

            return list;
        }

        //public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        //{
        //    Requires.NotNull(source, "source");

        //    return source.ToDictionary(m => m.Key, m => m.Value);
        //}

        public static bool IsEmpty<T>(this IEnumerable<T> @this)
        {
            Requires.Object(@this);

            return !@this.Any();
        }

        /// <summary>
        /// An order independent version of <see cref="Enumerable.SequenceEqual{TSource}(System.Collections.Generic.IEnumerable{TSource},System.Collections.Generic.IEnumerable{TSource})"/>.
        /// </summary>
        public static bool SetEqual<T>(this IEnumerable<T> @this, IEnumerable<T> y)
        {
            Requires.Object(@this);
            Requires.NotNull(y, "y");

            var objectsInX = @this.ToList();
            var objectsInY = y.ToList();

            if (objectsInX.Count() != objectsInY.Count()) {
                return false;
            }

            foreach (var objectInY in objectsInY) {
                if (!objectsInX.Contains(objectInY)) {
                    return false;
                }

                objectsInX.Remove(objectInY);
            }

            return !objectsInX.Any();
        }

        #region > Reduce <

        public static TAccumulate FoldLeft<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Requires.Object(@this);

            return @this.Aggregate(seed, accumulator);

            //TAccumulate acc = seed;

            //foreach (TSource item in @this) {
            //    acc = fun(acc, item);
            //}

            //return acc;
        }

        // foldBack en F# ou foldR en Haskell.
        public static TAccumulate FoldRight<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Requires.Object(@this);

            return @this.Reverse().Aggregate(seed, accumulator);
        }

        public static T Reduce<T>(
            this IEnumerable<T> @this,
            Func<T, T, T> accumulator)
        {
            Requires.Object(@this);

            return @this.Aggregate(accumulator);
        }

        #endregion
    }
}
