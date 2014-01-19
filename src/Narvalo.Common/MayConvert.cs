namespace Narvalo
{
    using Narvalo.Fx;

    public static class MayConvert
    {
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
