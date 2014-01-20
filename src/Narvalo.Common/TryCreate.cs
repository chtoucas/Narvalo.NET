namespace Narvalo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Mail;

    public static class TryCreate
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "La méthode retourne un booléen pour indiquer le succès ou l'échec.")]
        public static bool MailAddress(string value, out MailAddress result)
        {
            result = default(MailAddress);

            if (String.IsNullOrEmpty(value)) { return false; }

            try {
                result = new System.Net.Mail.MailAddress(value);
                return true;
            }
            catch (FormatException) {
                return false;
            }
        }
    }
}
