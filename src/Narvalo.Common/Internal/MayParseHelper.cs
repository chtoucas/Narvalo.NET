namespace Narvalo.Internal
{
    using Narvalo.Fx;

    static class MayParseHelper
    {
        public static Maybe<T> Parse<T>(string value, TryParse<T> fun)
        {
            if (value == null) { return Maybe<T>.None; }

            T result;
            return fun(value, out result) ? Maybe.Create(result) : Maybe<T>.None;
        }
    }
}
