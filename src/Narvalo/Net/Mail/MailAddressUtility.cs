namespace Narvalo.Net.Mail
{
    using System;
    using System.Net.Mail;
    using System.Text;
    using Narvalo.Fx;
    using Narvalo.Internal;

    // Cf.
    //  - http://cobisi.com/email-validation/.net-component
    //  - http://msdn.microsoft.com/en-us/library/01escwtf%28v=vs.110%29.aspx
    public static class MailAddressUtility
    {
        public static Outcome<MailAddress> Create(string value)
        {
            Requires.NotNullOrEmpty(value, "value");

            try {
                return Outcome.Success(new MailAddress(value));
            }
            catch (FormatException ex) {
                return Outcome.Failure<MailAddress>(ex);
            }
        }

        public static Outcome<MailAddress> Create(string value, string displayName)
        {
            Requires.NotNullOrEmpty(value, "value");

            try {
                return Outcome.Success(new MailAddress(value, displayName));
            }
            catch (FormatException ex) {
                return Outcome.Failure<MailAddress>(ex);
            }
        }

        public static Outcome<MailAddress> Create(
            string value, 
            string displayName, 
            Encoding displayNameEncoding)
        {
            Requires.NotNullOrEmpty(value, "value");
            Requires.NotNullOrEmpty(displayName, "displayName");

            try {
                return Outcome.Success(new MailAddress(value, displayName, displayNameEncoding));
            }
            catch (FormatException ex) {
                return Outcome.Failure<MailAddress>(ex);
            }
        }

        public static Maybe<MailAddress> MayParse(string value)
        {
            return MayParseHelper.Parse<MailAddress>(
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
