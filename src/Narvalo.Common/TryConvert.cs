namespace Narvalo
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static partial class TryConvert
    {
        /// <remarks>
        /// Ne marche pas de manière cohérente pour les enumérations de type Flags :
        /// http://msdn.microsoft.com/en-us/library/system.enum.isdefined.aspx
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "La méthode retourne un booléen pour indiquer le succès ou l'échec.")]
        public static bool ToEnum<TEnum>(object value, out TEnum result) where TEnum : struct
        {
            DebugCheck.IsEnum(typeof(TEnum));

            result = default(TEnum);

            if (Enum.IsDefined(typeof(TEnum), value)) {
                result = (TEnum)Enum.ToObject(typeof(TEnum), value);
                return true;
            }
            else {
                return false;
            }
        }
    }
}
