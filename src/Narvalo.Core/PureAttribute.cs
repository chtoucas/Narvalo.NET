// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Methods and classes marked with this attribute can be used within calls to Contract methods.
    /// Such methods not make any visible state changes.
    /// </summary>
    /// <remarks>Shim for the class System.Diagnostics.Contracts.ExcludeFromCodeCoverageAttribute.</remarks>
    [Conditional("CONTRACTS_FULL")]
    [AttributeUsage(
        AttributeTargets.Class
        | AttributeTargets.Constructor
        | AttributeTargets.Delegate
        | AttributeTargets.Event
        | AttributeTargets.Method
        | AttributeTargets.Parameter
        | AttributeTargets.Property,
        AllowMultiple = false,
        Inherited = true)]
    public sealed class PureAttribute : Attribute { }
}
