namespace Narvalo.Net
{
    using System.Net;
    using Narvalo.Fx;
    using Narvalo.Internal;

    public static class IPAddressUtility
    {
        public static Maybe<IPAddress> ToIPAddress(string value)
        {
            return MayParseHelper.Parse<IPAddress>(
                value,
                (string val, out IPAddress result) => IPAddress.TryParse(val, out result));
        }
    }
}