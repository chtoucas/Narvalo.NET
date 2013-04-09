namespace Narvalo.Web
{
    using System;

    [Flags]
    public enum HttpVersions
    {
        Http_1_0 = 1 << 0,
        Http_1_1 = 1 << 1,

        All = Http_1_0 | Http_1_1
    }
}
