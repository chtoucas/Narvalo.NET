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
        public static void Clear(this NameValueCollection nvc, IEnumerable<string> keys)
        {
            Requires.NotNull(nvc, "nvc");
            Requires.NotNull(keys, "keys");

            foreach (var key in keys) {
                nvc.Remove(key);
            }
        }

        public static T Feed<T>(this NameValueCollection nvc) where T : new()
        {
            Requires.NotNull(nvc, "nvc");

            var t = new T();
            Type type = t.GetType();

            foreach (var key in nvc.AllKeys) {
                PropertyInfo pi = type.GetProperty(key);
                if (pi != null) {
                    pi.SetValue(
                        t,
                        Convert.ChangeType(nvc[key], pi.PropertyType, CultureInfo.InvariantCulture),
                        null);
                }
            }

            return t;
        }
    }
}
