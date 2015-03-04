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
        #region Wrappers for XmlReader methods using a XName parameter.

        public static string ReadElementString(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.ReadElementString(name.LocalName, name.NamespaceName);
        }

        public static DateTime ReadElementContentAsDateTime(this XmlReader @this, XName name)
        {
            Require.Object(@this);
            Require.NotNull(name, "name");

            return @this.ReadElementContentAsDateTime(name.LocalName, name.NamespaceName);
        }

        #endregion
    }
}
