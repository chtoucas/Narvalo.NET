namespace Narvalo.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Flags]
    public enum HttpVersions
    {
        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        Http_1_0 = 1 << 0,
        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        Http_1_1 = 1 << 1,

        All = Http_1_0 | Http_1_1
    }
}
