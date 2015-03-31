// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The Narvalo namespace contains classes dealing with Code Contracts
    /// and argument validation.
    /// </summary>
    /// <remarks>
    /// <para>We provide four static classes to perform argument validation:
    /// <list type="bullet">
    /// <item><see cref="Enforce"/> throws an <see cref="System.ArgumentException"/>
    /// on failure.</item>
    /// <item><see cref="Require"/> uses Code Contracts preconditions
    /// and throws <see cref="System.ArgumentException"/> on failure.</item>
    /// <item><see cref="Promise"/> uses Code Contracts preconditions
    /// and throws an unrecoverable exception on failure in debug builds.</item>
    /// <item><see cref="Check"/> throws an unrecoverable exception on failure
    /// in debug builds.</item>
    /// </list>
    /// Only the first two will survive in retail builds.</para>
    /// <para>Each one serves a different purpose.</para>
    /// <para>For parameter validation of public methods, use <see cref="Require"/>
    /// and <see cref="Enforce"/> which provides support for conditions not yet handled
    /// by Code Contracts.</para>
    /// <para>For parameter validation of private methods, use <see cref="Promise"/>.</para>
    /// <para>For parameter validation of protected overriden methods in sealed classes,
    /// when you know for certain that *ALL* callers satisfy the condition
    /// and when you own all base classes, use <see cref="Promise"/>.
    /// When the base method declares a contract use <see cref="Check"/> instead.</para>
    /// <para>We also provide two static classes to help with Code Contracts:
    /// <list type="bullet">
    /// <item><see cref="Acknowledge"/> provides Code Contracts abbreviators and helpers.</item>
    /// <item><see cref="Assume"/> focuses on assumptions.</item>
    /// </list>
    /// </para>
    /// </remarks>
    [CompilerGenerated]
    internal static class NamespaceDoc { }
}
