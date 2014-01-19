namespace Narvalo
{
    using Narvalo.Fx;
    using Narvalo.Internal;

    public static partial class MayParse
    {
        internal static Maybe<T> MayParseCore<T>(string value, TryParse<T> fun)
        {
            if (value == null) { return Maybe<T>.None; }

            T result;
            return fun(value, out result) ? Maybe.Create(result) : Maybe<T>.None;
        }
    }
}