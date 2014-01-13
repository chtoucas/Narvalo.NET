namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Reflection;
    using Narvalo.Diagnostics;

    public static class NameValueCollectionExtensions
    {
        public static void Clear(this NameValueCollection @this, IEnumerable<string> keys)
        {
            Requires.Object(@this);
            Requires.NotNull(keys, "keys");

            foreach (var key in keys) {
                @this.Remove(key);
            }
        }

        public static T Feed<T>(this NameValueCollection @this) where T : new()
        {
            Requires.Object(@this);

            var t = new T();
            Type type = t.GetType();

            foreach (var key in @this.AllKeys) {
                PropertyInfo pi = type.GetProperty(key);
                if (pi != null) {
                    pi.SetValue(
                        t,
                        Convert.ChangeType(@this[key], pi.PropertyType, CultureInfo.InvariantCulture),
                        null);
                }
            }

            return t;
        }
    }
}
