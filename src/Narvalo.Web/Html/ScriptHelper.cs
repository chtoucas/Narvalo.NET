// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Narvalo.Web.Internal;

    public static class ScriptHelper
    {
        public static IHtmlString Render(Uri scriptUri)
        {
            Contract.Requires(scriptUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Render(scriptUri, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Render(Uri scriptUri, string scriptType)
        {
            Contract.Requires(scriptUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Render(scriptUri, scriptType, (IDictionary<string, object>)null);
        }

        public static IHtmlString Render(Uri scriptUri, string scriptType, object attributes)
        {
            Contract.Requires(scriptUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Render(scriptUri, scriptType, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Render(
            Uri scriptUri,
            string scriptType,
            IDictionary<string, object> attributes)
        {
            Require.NotNull(scriptUri, "scriptUri");
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var tag = new TagBuilder("script");
            tag.MergeAttribute("src", scriptUri.ToProtocolRelativeString());

            if (!String.IsNullOrEmpty(scriptType))
            {
                tag.MergeAttribute("type", scriptType);
            }

            tag.MergeAttributes(attributes, replaceExisting: true);

            return tag.ToHtmlString();
        }
    }
}
