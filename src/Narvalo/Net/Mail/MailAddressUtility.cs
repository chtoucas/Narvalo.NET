namespace Narvalo.Net.Mail
{
    using System;
    using System.Net.Mail;
    using Narvalo.Fx;

    public static class MailAddressUtility
    {
        public static Outcome<MailAddress> Create(string value)
        {
            try {
                return Outcome<MailAddress>.Success(new MailAddress(value));
            }
            catch (FormatException ex) {
                return Outcome<MailAddress>.Failure(ex);
            }
        }

        public static Maybe<MailAddress> MayParse(string value)
        {
            return Narvalo.MayParse.MayParseHelper<MailAddress>(
                value,
                (string val, out MailAddress result) => TryParse(val, out result)
            );
        }

        public static bool TryParse(string value, out MailAddress result)
        {
            result = default(MailAddress);

            if (value == null || value.Length == 0) { return false; }

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
