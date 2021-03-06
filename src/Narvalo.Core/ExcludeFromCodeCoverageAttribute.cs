﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    /// <summary>
    /// Specifies that the attributed code should be excluded from code coverage information.
    /// </summary>
    /// <remarks>Shim for System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute.
    /// It seems that this attribute will be part of .NET Standard 2.0.</remarks>
    [AttributeUsage(
        AttributeTargets.Class
        | AttributeTargets.Constructor
        | AttributeTargets.Event
        | AttributeTargets.Method
        | AttributeTargets.Property
        | AttributeTargets.Struct,
        Inherited = false,
        AllowMultiple = false)]
    public sealed class ExcludeFromCodeCoverageAttribute : Attribute { }
}
