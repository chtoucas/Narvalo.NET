﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System.IO;
    using System.Web.Util;

    // WARNING: Cet encodeur ne doit pas être utilisé dans un site web ouvert à toute modification non controlée,
    // et même dans ce cas...
    public class RawHttpEncoder : HttpEncoder
    {
        protected override void HtmlAttributeEncode(string value, TextWriter output)
        {
            output.Write(value);
        }

        protected override void HtmlEncode(string value, TextWriter output)
        {
            output.Write(value);
        }
    }
}