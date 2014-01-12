namespace Narvalo
{
    using System;
    using System.Globalization;
    using Narvalo.Fx;

    public static class BooleanUtility
    {
        public static bool TryParse(string value, out bool result)
        {
            return TryParse(value, BooleanStyles.Default, out result);
        }

        public static bool TryParse(string value, BooleanStyles style, out bool result)
        {
            result = default(Boolean);

            if (value == null) { return false; }

            var val = value.Trim();

            if (val.Length == 0) {
                if (style.HasFlag(BooleanStyles.EmptyIsFalse)) {
                    result = false;
                    return true;
                }
                else {
                    return false;
                }
            }

            if (style.HasFlag(BooleanStyles.Literal)) {
                // NB: cette méthode n'est pas sensible à la casse de "value".
                if (Boolean.TryParse(val, out result)) {
                    return true;
                }
            }

            if (style.HasFlag(BooleanStyles.Integer)) {
                Maybe<bool> b = MayParse
                    .ToInt32(val, NumberStyles.Integer, CultureInfo.InvariantCulture)
                    .Map(_ => _ > 0);

                if (b.IsSome) {
                    result = b.Value;
                    return true;
                }
            }

            if (style.HasFlag(BooleanStyles.HtmlInput)) {
                throw new NotImplementedException();
            }

            return false;
        }
    }
}
