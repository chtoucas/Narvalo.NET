﻿namespace Narvalo.Web.Html
{
    using System;
    using System.Web;

    public static partial class Tag
    {
        public static IHtmlString LoremIpsum()
        {
            string ipsum = @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

            return new HtmlString(ipsum);
        }

        static string ToProtocolLessUriString_(Uri uri)
        {
            return uri.ToString().Replace("http:", String.Empty);
        }
    }
}
