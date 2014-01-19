namespace Narvalo
{
    using Narvalo.Internal;

    public static partial class Create
    {
        internal static T? CreateCore<T>(string value, TryParse<T> fun) where T : struct
        {
            if (value == null) { return null; }

            T result;
            return fun(value, out result) ? result : (T?)null;
        }
    }
}
