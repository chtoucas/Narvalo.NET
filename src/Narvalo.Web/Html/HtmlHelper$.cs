// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Provides extension methods for <see cref="HtmlHelper"/>.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        private static readonly HtmlString s_Lipsum
            = new HtmlString(
                "Lorem ipsum dolor sit amet, consectetur adipisicing elit, "
                + "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. "
                + "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris "
                + "nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in "
                + "reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. "
                + "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia "
                + "deserunt mollit anim id est laborum.");

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this",
            Justification = "[Intentionally] We use an extension method to improve the accessibility of this method.")]
        public static IHtmlString LoremIpsum(this HtmlHelper @this)
        {
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return s_Lipsum;
        }
    }
}
