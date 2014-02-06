// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using System.Xml.Linq;
    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="System.Xml.Linq.XAttribute"/>.
    /// </summary>
    public static class XAttributeExtensions
    {
        public static T Select<T>(this XAttribute @this, Func<string, T> selector)
        {
            Require.Object(@this);
            Require.NotNull(selector, "selector");

            return selector.Invoke(@this.Value);
        }

        public static Maybe<T> MayParseValue<T>(this XAttribute @this, Func<string, Maybe<T>> parserM)
        {
            Require.Object(@this);
            Require.NotNull(parserM, "fun");

            var x = from q in @this select new MailAddress(q);

            return parserM.Invoke(@this.Value);
        }

        //[Obsolete]
        //public static T ParseValue<T>(this XAttribute @this, Func<string, Maybe<T>> parserM)
        //{
        //    Require.Object(@this);
        //    Require.NotNull(parserM, "parserM");

        //    return parserM.Invoke(@this.Value).ValueOrThrow(() => new XmlException(
        //        Format.CurrentCulture(
        //            SR.XElement_MalformedAttributeValueFormat,
        //            @this.Name.LocalName,
        //            ((IXmlLineInfo)@this).LineNumber)));
        //}
    }
}
