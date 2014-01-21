namespace Narvalo.Web
{
    using System;

    [Flags]
    public enum HttpVersions
    {
        HttpV10 = 1 << 0,
        HttpV11 = 1 << 1,

        All = HttpV10 | HttpV11
    }
}
