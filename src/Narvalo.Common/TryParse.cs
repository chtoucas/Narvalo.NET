namespace Narvalo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Net.Mail;
    using Narvalo.Fx;

    public static partial class TryParse
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "La méthode retourne un booléen pour indiquer le succès ou l'échec.")]
        public static bool ToBoolean(string value, out bool result)
        {
            return ToBoolean(value, BooleanStyles.Default, out result);
        }

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "La méthode retourne un booléen pour indiquer le succès ou l'échec.")]
        public static bool ToBoolean(string value, BooleanStyles style, out bool result)
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
                // NB: Cette méthode n'est pas sensible à la casse de "value".
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
                return value == "on";
            }

            return false;
        }

#if NET_35

        /// <example>
        /// <code>
        /// MyEnum result;
        /// TryParse.ToEnum&lt;MyEnum&gt;("ActualValue", out result);
        /// </code>
        /// </example>
        public static bool ToEnum<TEnum>(string value, out TEnum result) where TEnum : struct
        {
            return ToEnum<TEnum>(value, false /* ignoreCase */, out result);
        }

        /// <example>
        /// <code>
        /// MyEnum result;
        /// TryParse.ToEnum&lt;MyEnum&gt;("actualvalue", out result, true /* ignoreCase */);
        /// </code>
        /// </example>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "La méthode retourne un booléen pour indiquer le succès ou l'échec.")]
        public static bool ToEnum<TEnum>(string value, bool ignoreCase, out TEnum result)
            where TEnum : struct
        {
            // FIXME: ne marche pas de manière cohérente pour les enum's de type Flags.
            @Check.IsEnum(typeof(TEnum));

            //if (Attribute.IsDefined(type, typeof(FlagsAttribute))) {
            //  throw Failure.Argument();
            //}

            result = default(TEnum);

            if (String.IsNullOrEmpty(value)) {
                return false;
            }

            StringComparison comparison = ignoreCase
                ? StringComparison.InvariantCultureIgnoreCase
                : StringComparison.InvariantCulture;

            foreach (string name in Enum.GetNames(typeof(TEnum))) {
                if (name.Equals(value, comparison)) {
                    result = (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);

                    return true;
                }
            }

            return false;
        }

#endif

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "La méthode retourne un booléen pour indiquer le succès ou l'échec.")]
        public static bool ToMailAddress(string value, out MailAddress result)
        {
            result = default(MailAddress);

            if (String.IsNullOrEmpty(value)) { return false; }

            try {
                result = new MailAddress(value);
                return true;
            }
            catch (FormatException) {
                return false;
            }
        }
    }
}
