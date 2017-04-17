// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    /// <summary>
    /// Specifies that the attributed code should be excluded from the API reference.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class
        | AttributeTargets.Constructor
        | AttributeTargets.Event
        | AttributeTargets.Method
        | AttributeTargets.Property
        | AttributeTargets.Struct,
        Inherited = false,
        AllowMultiple = false)]
    public sealed class ExcludeFromApiReferenceAttribute : Attribute { }
}
