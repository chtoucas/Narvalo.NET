// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Xml.Linq;

    /// <summary>
    /// Provides extension methods for <see cref="System.Xml.Linq.XElement"/>.
    /// </summary>
    public static class XElementExtensions
    {
        //[Obsolete]
        //public static T ParseValue<T>(this XElement @this, Func<string, Maybe<T>> parserM)
        //{
        //    Require.Object(@this);
        //    Require.NotNull(parserM, "parserM");

        //    return parserM.Invoke(@this.Value).ValueOrThrow(() => new XmlException(
        //        Format.CurrentCulture(
        //            SR.XElement_MalformedElementValueFormat,
        //            @this.Name.LocalName,
        //            ((IXmlLineInfo)@this).LineNumber)));
        //}
    }
}
