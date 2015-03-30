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
    /// <item><see cref="Enforce"/> throws <see cref="System.ArgumentException"/>.</item>
    /// <item><see cref="Require"/> uses Code Contracts preconditions 
    /// and throws <see cref="System.ArgumentException"/>.</item>
    /// <item><see cref="Promise"/> uses Code Contracts preconditions 
    /// and throws unrecoverable exceptions in debug builds.</item>
    /// <item><see cref="Check"/> throws unrecoverable exceptions in debug builds.</item>
    /// </list>
    /// Each one serves a different purpose but only the first two will survive
    /// in retail builds.</para>
    /// <para>For parameter validation of public methods, use <see cref="Require"/> 
    /// and <see cref="Enforce"/> which provides support for conditions not yet handled 
    /// by Code Contracts.</para>
    /// <para>For parameter validation of private methods, use <see cref="Promise"/>.</para>
    /// <para>For parameter validation of protected methods in sealed classes,
    /// only when you know for certain that ALL callers satisfy the condition,
    /// use <see cref="Promise"/> when the base method does not declare any contract; 
    /// otherwise use <see cref="Check"/>.</para>
    /// <para>We provide two static classes to help with Code Contracts:
    /// <list type="bullet">
    /// <item><see cref="Acknowledge"/> provides Code Contracts abbreviators.</item>
    /// <item><see cref="Assume"/> is for assumptions.</item>
    /// </list>
    /// </para>
    /// </remarks>
    [CompilerGenerated]
    internal static class NamespaceDoc { }
}
