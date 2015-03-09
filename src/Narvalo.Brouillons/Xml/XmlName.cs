// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System;
    using System.Xml.Linq;

    [Serializable]
    public sealed class XmlName //: IEquatable<XmlName>
    {
        readonly XName _inner;

        XmlName(XName inner)
        {
            _inner = inner;
        }

        /// <summary>
        /// Gets the local (unqualified) part of the name.
        /// </summary>
        /// <seealso cref="System.Xml.Linq.XName.Namespace"/>
        public string LocalName
        {
            get { return _inner.LocalName; }
        }

        /// <summary>
        /// Gets the namespace of the name.
        /// </summary>
        public string Namespace
        {
            get { return _inner.NamespaceName; }
        }

        /// <summary>
        /// Gets the underlying XName.
        /// </summary>
        public XName XName
        {
            get { return _inner; }
        }

        public static XmlName Get(string expandedName)
        {
            return new XmlName(XName.Get(expandedName));
        }

        public static XmlName Get(string localName, string namespaceName)
        {
            return new XmlName(XName.Get(localName, namespaceName));
        }

        //public bool Equals(XmlName other)
        //{
        //    throw new NotImplementedException();
        //}
    }

    //public sealed class XNameLLL : IEquatable<XNameLLL> //, ISerializable
    //{
    //    XNamespace ns;
    //    string localName;
    //    int hashCode;

    //    /// <summary>
    //    /// Constructor, internal so that external users must go through the Get() method to create an XName.
    //    /// </summary>
    //    internal XNameLLL(XNamespace ns, string localName)
    //    {
    //        this.ns = ns;
    //        this.localName = XmlConvert.VerifyNCName(localName);
    //        this.hashCode = ns.GetHashCode() ^ localName.GetHashCode();
    //    }

    //    /// <summary>
    //    /// Gets the local (unqualified) part of the name.
    //    /// </summary>
    //    /// <seealso cref="XNameLLL.Namespace"/>
    //    public string LocalName
    //    {
    //        get { return localName; }
    //    }

    //    /// <summary>
    //    /// Gets the namespace of the name.
    //    /// </summary>
    //    public XNamespace Namespace
    //    {
    //        get { return ns; }
    //    }

    //    /// <summary>
    //    /// Gets the namespace name part of the name.
    //    /// </summary>
    //    public string NamespaceName
    //    {
    //        get { return ns.NamespaceName; }
    //    }

    //    /// <summary>
    //    /// Returns the expanded XML name in the format: {namespaceName}localName.
    //    /// </summary>
    //    public override string ToString()
    //    {
    //        if (ns.NamespaceName.Length == 0) return localName;
    //        return "{" + ns.NamespaceName + "}" + localName;
    //    }

    //    ///// <summary>
    //    ///// Returns an <see cref="XName"/> object created from the specified expanded name.
    //    ///// </summary>
    //    ///// <param name="expandedName">
    //    ///// A string containing an expanded XML name in the format: {namespace}localname.
    //    ///// </param>
    //    ///// <returns>
    //    ///// An <see cref="XName"/> object constructed from the specified expanded name.
    //    ///// </returns>
    //    //public static XName Get(string expandedName)
    //    //{
    //    //    if (expandedName == null) throw new ArgumentNullException("expandedName");
    //    //    if (expandedName.Length == 0) throw new ArgumentException("InvalidExpandedName", expandedName);
    //    //    if (expandedName[0] == '{') {
    //    //        int i = expandedName.LastIndexOf('}');
    //    //        if (i <= 1 || i == expandedName.Length - 1) throw new ArgumentException("InvalidExpandedName", expandedName);
    //    //        return XNamespace.Get(expandedName, 1, i - 1).GetName(expandedName, i + 1, expandedName.Length - i - 1);
    //    //    }
    //    //    else {
    //    //        return XNamespace.None.GetName(expandedName);
    //    //    }
    //    //}

    //    ///// <summary>
    //    ///// Returns an <see cref="XName"/> object from a local name and a namespace.
    //    ///// </summary>
    //    ///// <param name="localName">A local (unqualified) name.</param>
    //    ///// <param name="namespaceName">An XML namespace.</param>
    //    ///// <returns>An XName object created from the specified local name and namespace.</returns>
    //    //public static XName Get(string localName, string namespaceName)
    //    //{
    //    //    return XNamespace.Get(namespaceName).GetName(localName);
    //    //}

    //    /// <summary>
    //    /// Converts a string formatted as an expanded XML name ({namespace}localname) to an XName object.
    //    /// </summary>
    //    /// <param name="expandedName">A string containing an expanded XML name in the format: {namespace}localname.</param>
    //    /// <returns>An XName object constructed from the expanded name.</returns>        
    //    //[CLSCompliant(false)]
    //    //public static implicit operator XName(string expandedName)
    //    //{
    //    //    return expandedName != null ? Get(expandedName) : null;
    //    //}

    //    /// <summary>
    //    /// Determines whether the specified <see cref="XNameLLL"/> is equal to the current <see cref="XNameLLL"/>.
    //    /// </summary>
    //    /// <param name="obj">The XName to compare to the current XName.</param>
    //    /// <returns>
    //    /// true if the specified <see cref="XNameLLL"/> is equal to the current XName; otherwise false.
    //    /// </returns>
    //    /// <remarks>
    //    /// For two <see cref="XNameLLL"/> objects to be equal, they must have the same expanded name.
    //    /// </remarks>
    //    public override bool Equals(object obj)
    //    {
    //        return (object)this == obj;
    //    }

    //    /// <summary>
    //    /// Serves as a hash function for <see cref="XNameLLL"/>. GetHashCode is suitable 
    //    /// for use in hashing algorithms and data structures like a hash table.  
    //    /// </summary>
    //    public override int GetHashCode()
    //    {
    //        return hashCode;
    //    }

    //    // The overloads of == and != are included to enable comparisons between
    //    // XName and string (e.g. element.Name == "foo"). C#'s predefined reference
    //    // equality operators require one operand to be convertible to the type of
    //    // the other through reference conversions only and do not consider the
    //    // implicit conversion from string to XName.

    //    /// <summary>
    //    /// Returns a value indicating whether two instances of <see cref="XNameLLL"/> are equal.
    //    /// </summary>
    //    /// <param name="left">The first XName to compare.</param>
    //    /// <param name="right">The second XName to compare.</param>
    //    /// <returns>true if left and right are equal; otherwise false.</returns>
    //    /// <remarks>
    //    /// This overload is included to enable the comparison between
    //    /// an instance of XName and string.
    //    /// </remarks>
    //    public static bool operator ==(XNameLLL left, XNameLLL right)
    //    {
    //        return (object)left == (object)right;
    //    }

    //    /// <summary>
    //    /// Returns a value indicating whether two instances of <see cref="XNameLLL"/> are not equal.
    //    /// </summary>
    //    /// <param name="left">The first XName to compare.</param>
    //    /// <param name="right">The second XName to compare.</param>
    //    /// <returns>true if left and right are not equal; otherwise false.</returns>
    //    /// <remarks>
    //    /// This overload is included to enable the comparison between
    //    /// an instance of XName and string.
    //    /// </remarks>
    //    public static bool operator !=(XNameLLL left, XNameLLL right)
    //    {
    //        return (object)left != (object)right;
    //    }

    //    /// <summary>
    //    /// Indicates whether the current <see cref="XNameLLL"/> is equal to 
    //    /// the specified <see cref="XNameLLL"/>
    //    /// </summary>
    //    /// <param name="other">The <see cref="XNameLLL"/> to compare with the
    //    /// current <see cref="XNameLLL"/></param> 
    //    /// <returns>
    //    /// Returns true if the current <see cref="XNameLLL"/> is equal to
    //    /// the specified <see cref="XNameLLL"/>. Returns false otherwise. 
    //    /// </returns>
    //    bool IEquatable<XNameLLL>.Equals(XNameLLL other)
    //    {
    //        return (object)this == (object)other;
    //    }

    //    ///// <summary>
    //    ///// Populates a <see cref="SerializationInfo"/> with the data needed to
    //    ///// serialize the <see cref="XName"/>
    //    ///// </summary>
    //    ///// <param name="info">The <see cref="SerializationInfo"/> to populate with data</param>
    //    ///// <param name="context">The destination for this serialization</param>
    //    //[System.Security.Permissions.SecurityPermission(
    //    //    System.Security.Permissions.SecurityAction.LinkDemand,
    //    //    Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
    //    //void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    //    //{
    //    //    if (info == null) throw new ArgumentNullException("info");
    //    //    info.AddValue("name", ToString());
    //    //    info.SetType(typeof(NameSerializer));
    //    //}
    //}
}
