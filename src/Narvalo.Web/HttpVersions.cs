namespace Narvalo.Web
{
    using System;

    [Flags]
    [Serializable]
    public enum HttpVersions
    {
        HttpV10 = 1 << 0,
        HttpV11 = 1 << 1,

        All = HttpV10 | HttpV11
    }
}
