// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Provides extension methods for <see cref="XmlReader"/>.
    /// </summary>
    public static class XmlReaderExtensions
    {
        #region Wrappers for XmlReader methods using a XName parameter.

        public static string GetAttribute(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));

            return @this.GetAttribute(name.LocalName, name.NamespaceName);
        }

        public static bool IsStartElement(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));

            return @this.IsStartElement(name.LocalName, name.NamespaceName);
        }

        public static bool MoveToAttribute(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));

            return @this.MoveToAttribute(name.LocalName, name.NamespaceName);
        }

        public static object ReadElementContentAs(
            this XmlReader @this,
            Type returnType,
            IXmlNamespaceResolver namespaceResolver,
            XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));
            Demand.NotNull(returnType);

            return @this.ReadElementContentAs(returnType, namespaceResolver, name.LocalName, name.NamespaceName);
        }

        public static bool ReadElementContentAsBoolean(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));

            return @this.ReadElementContentAsBoolean(name.LocalName, name.NamespaceName);
        }

        public static decimal ReadElementContentAsDecimal(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));

            return @this.ReadElementContentAsDecimal(name.LocalName, name.NamespaceName);
        }

        public static double ReadElementContentAsDouble(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));

            return @this.ReadElementContentAsDouble(name.LocalName, name.NamespaceName);
        }

        public static int ReadElementContentAsInt32(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));

            return @this.ReadElementContentAsInt(name.LocalName, name.NamespaceName);
        }

        public static long ReadElementContentAsInt64(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));

            return @this.ReadElementContentAsLong(name.LocalName, name.NamespaceName);
        }

        public static object ReadElementContentAsObject(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));

            return @this.ReadElementContentAsObject(name.LocalName, name.NamespaceName);
        }

        public static float ReadElementContentAsSingle(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));

            return @this.ReadElementContentAsFloat(name.LocalName, name.NamespaceName);
        }

        public static string ReadElementContentAsString(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));

            return @this.ReadElementContentAsString(name.LocalName, name.NamespaceName);
        }

        public static void ReadStartElement(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));

            @this.ReadStartElement(name.LocalName, name.NamespaceName);
        }

        public static bool ReadToDescendant(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));

            return @this.ReadToDescendant(name.LocalName, name.NamespaceName);
        }

        public static bool ReadToFollowing(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));

            return @this.ReadToFollowing(name.LocalName, name.NamespaceName);
        }

        public static bool ReadToNextSibling(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, nameof(name));

            return @this.ReadToNextSibling(name.LocalName, name.NamespaceName);
        }

        #endregion

        #region Wrappers not available with PCL.

        ////public static DateTime ReadElementContentAsDateTime(this XmlReader @this, XName name)
        ////{
        ////    Require.Object(@this);
        ////    Require.NotNull(name, "name");

        ////    return @this.ReadElementContentAsDateTime(name.LocalName, name.NamespaceName);
        ////}

        ////public static string ReadElementString(this XmlReader @this, XName name)
        ////{
        ////    Require.Object(@this);
        ////    Require.NotNull(name, "name");

        ////    return @this.ReadElementString(name.LocalName, name.NamespaceName);
        ////}

        #endregion
    }
}
