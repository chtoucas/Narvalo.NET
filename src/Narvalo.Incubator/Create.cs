// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

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
        public static Output<MailAddress> MailAddress(string value)
        {
            return Result<MailAddress>.Catch<FormatException>(() => new MailAddress(value));
        }

        public static Output<MailAddress> MailAddress(string value, string displayName)
        {
            return Result<MailAddress>.Catch<FormatException>(() => new MailAddress(value, displayName));
        }

        public static Output<MailAddress> MailAddress(
            string value,
            string displayName,
            Encoding displayNameEncoding)
        {
            return Result<MailAddress>.Catch<FormatException>(() => new MailAddress(value, displayName, displayNameEncoding));
        }
    }
}
