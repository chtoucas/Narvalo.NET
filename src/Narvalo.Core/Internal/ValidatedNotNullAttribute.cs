// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;

    /// <summary>
    /// Decorating a parameter with this attribute informs Code Analysis
    /// that the method is validating the parameter for <see langword="null"/>.
    /// This will silence the CA1062 warning.
    /// <seealso href="http://geekswithblogs.net/terje/archive/2010/10/14/making-static-code-analysis-and-code-contracts-work-together-or.aspx" />
    /// </summary>
    /// <remarks>
    /// When performing Static Contracts checking, this attribute must be public.
    /// This will silence the CC1038 error.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
#if CONTRACTS_FULL
    // Preconditions must be public.
    public
#else
    internal
#endif
    sealed class ValidatedNotNullAttribute : Attribute { }
}
