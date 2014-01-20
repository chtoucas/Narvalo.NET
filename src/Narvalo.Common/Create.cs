namespace Narvalo
{
    using System;
    using System.Net.Mail;
    using System.Text;
    using Narvalo.Fx;

    // Cf.
    //  - http://cobisi.com/email-validation/.net-component
    //  - http://msdn.microsoft.com/en-us/library/01escwtf%28v=vs.110%29.aspx
    public static class Create
    {
        public static Outcome<MailAddress> MailAddress(string value)
        {
            Require.NotNullOrEmpty(value, "value");

            try {
                return Outcome.Create(new MailAddress(value));
            }
            catch (FormatException ex) {
                return Outcome.Failure<MailAddress>(ex);
            }
        }

        public static Outcome<MailAddress> MailAddress(string value, string displayName)
        {
            Require.NotNullOrEmpty(value, "value");

            try {
                return Outcome.Create(new MailAddress(value, displayName));
            }
            catch (FormatException ex) {
                return Outcome.Failure<MailAddress>(ex);
            }
        }

        public static Outcome<MailAddress> MailAddress(
            string value, 
            string displayName, 
            Encoding displayNameEncoding)
        {
            Require.NotNullOrEmpty(value, "value");
            Require.NotNullOrEmpty(displayName, "displayName");

            try {
                return Outcome.Create(new MailAddress(value, displayName, displayNameEncoding));
            }
            catch (FormatException ex) {
                return Outcome.Failure<MailAddress>(ex);
            }
        }
    }
}
