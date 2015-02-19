// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Provides extension methods for <see cref="System.Xml.XmlReader"/>.
    /// </summary>
    /// <remarks>
    /// The PCL version of System.Xml does not include the full version of XmlReader.
    /// </remarks>
    public static class XmlReaderExtensions
    {
        #region Methods that simply wrap XmlReader methods for use with an XName parameter.

        public static string GetAttribute(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.GetAttribute(name.LocalName, name.NamespaceName);
        }

        public static bool IsStartElement(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.IsStartElement(name.LocalName, name.NamespaceName);
        }

        public static bool MoveToAttribute(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.MoveToAttribute(name.LocalName, name.NamespaceName);
        }

        public static object ReadElementContentAs(
            this XmlReader @this,
            Type returnType,
            IXmlNamespaceResolver namespaceResolver,
            XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.ReadElementContentAs(returnType, namespaceResolver, name.LocalName, name.NamespaceName);
        }

        public static bool ReadElementContentAsBoolean(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.ReadElementContentAsBoolean(name.LocalName, name.NamespaceName);
        }

        public static decimal ReadElementContentAsDecimal(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.ReadElementContentAsDecimal(name.LocalName, name.NamespaceName);
        }

        public static double ReadElementContentAsDouble(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.ReadElementContentAsDouble(name.LocalName, name.NamespaceName);
        }

        public static int ReadElementContentAsInt32(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.ReadElementContentAsInt(name.LocalName, name.NamespaceName);
        }

        public static long ReadElementContentAsInt64(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.ReadElementContentAsLong(name.LocalName, name.NamespaceName);
        }

        public static object ReadElementContentAsObject(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.ReadElementContentAsObject(name.LocalName, name.NamespaceName);
        }

        public static float ReadElementContentAsSingle(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.ReadElementContentAsFloat(name.LocalName, name.NamespaceName);
        }

        public static string ReadElementContentAsString(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.ReadElementContentAsString(name.LocalName, name.NamespaceName);
        }

        public static void ReadStartElement(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            @this.ReadStartElement(name.LocalName, name.NamespaceName);
        }

        public static bool ReadToDescendant(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.ReadToDescendant(name.LocalName, name.NamespaceName);
        }

        public static bool ReadToFollowing(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.ReadToFollowing(name.LocalName, name.NamespaceName);
        }

        public static bool ReadToNextSibling(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.ReadToNextSibling(name.LocalName, name.NamespaceName);
        }

        #endregion
    }
}
