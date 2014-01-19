namespace Narvalo
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using Narvalo.Fx;

    public static partial class MayParse
    {
        public static Maybe<IPAddress> ToIPAddress(string value)
        {
            return MayParseCore(
                value,
                (string val, out IPAddress result) => IPAddress.TryParse(val, out result));
        }

        public static Maybe<MailAddress> ToMailAddress(string value)
        {
            return MayParseCore(
                value,
                (string val, out MailAddress result) => TryParse.ToMailAddress(val, out result));
        }

        public static Maybe<Uri> ToUri(string value, UriKind uriKind)
        {
            // NB: Uri.TryCreate accepte les chaînes vides.
            if (String.IsNullOrEmpty(value)) {
                return Maybe<Uri>.None;
            }

            return MayParseCore(
                value,
                (string val, out Uri result) => Uri.TryCreate(val, uriKind, out result));
        }
    }
}