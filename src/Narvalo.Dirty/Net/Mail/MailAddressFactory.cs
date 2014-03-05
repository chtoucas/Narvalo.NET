// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Net.Mail
{
    using System;
    using System.Net.Mail;
    using System.Text;
    using Narvalo.Fx;

    /*
     * References
     * ----------
     * + http://cobisi.com/email-validation/.net-component
     * + http://msdn.microsoft.com/en-us/library/01escwtf%28v=vs.110%29.aspx
     */

    public static class MailAddressFactory
    {
        public static Output<MailAddress> Create(string value)
        {
            return Make<MailAddress>.Catch<FormatException>(() => new MailAddress(value));
        }

        public static Output<MailAddress> Create(string value, string displayName)
        {
            return Make<MailAddress>.Catch<FormatException>(() => new MailAddress(value, displayName));
        }

        public static Output<MailAddress> Create(
            string value,
            string displayName,
            Encoding displayNameEncoding)
        {
            return Make<MailAddress>.Catch<FormatException>(() => new MailAddress(value, displayName, displayNameEncoding));
        }
    }
}
