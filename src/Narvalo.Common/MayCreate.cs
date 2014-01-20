namespace Narvalo
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using Narvalo.Fx;
    using Narvalo.Internal;

    public static class MayCreate
    {
        public static Maybe<IPAddress> IPAddress(string value)
        {
            return MayCreateCore(
                value,
                (string val, out IPAddress result) => System.Net.IPAddress.TryParse(val, out result));
        }

        public static Maybe<MailAddress> MailAddress(string value)
        {
            return MayCreateCore(
                value,
                (string val, out MailAddress result) => TryCreate.MailAddress(val, out result));
        }

        public static Maybe<Uri> Uri(string value, UriKind uriKind)
        {
            // NB: Uri.TryCreate accepte les chaînes vides.
            if (String.IsNullOrEmpty(value)) {
                return Maybe<Uri>.None;
            }

            return MayCreateCore(
                value,
                (string val, out Uri result) => System.Uri.TryCreate(val, uriKind, out result));
        }

        internal static Maybe<T> MayCreateCore<T>(string value, TryCreate<T> fun) where T : class
        {
            if (value == null) { return Maybe<T>.None; }

            T result;
            return fun(value, out result) ? Maybe.Create(result) : Maybe<T>.None;
        }
    }
}