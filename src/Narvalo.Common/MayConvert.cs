namespace Narvalo
{
    using Narvalo.Fx;

    public static class MayConvert
    {
        /// <example>
        /// <code>
        /// Maybe&lt;MyEnum&gt; value = MayConvert.ToEnum&lt;MyEnum&gt;(1);
        /// </code>
        /// </example>
        public static Maybe<TEnum> ToEnum<TEnum>(object value) where TEnum : struct
        {
            TEnum result;
            if (!TryConvert.ToEnum<TEnum>(value, out result)) {
                return Maybe<TEnum>.None;
            }
            
            return Maybe.Create(result);
        }
    }
}
