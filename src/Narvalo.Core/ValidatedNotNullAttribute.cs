// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    /// <summary>
    /// Decorating a parameter with this attribute informs the Code Analysis tool
    /// that the method is validating the parameter against <see langword="null"/> value.
    /// </summary>
    /// <remarks>
    /// <para>Using the parameter suppresses the CA1062 warning.</para>
    /// <para>Public visibility is mandatory for preconditions in Require.
    /// If Code Contracts are enabled, this attribute is used by the Require class for 
    /// writing preconditions. Therefore, to be able to perform Static Contracts checkings,
    /// this attribute MUST remain public. Not doing so would lead to a CC1038 error.</para>
    /// </remarks>
    /// <seealso href="http://geekswithblogs.net/terje/archive/2010/10/14/making-static-code-analysis-and-code-contracts-work-together-or.aspx" />
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class ValidatedNotNullAttribute : Attribute { }
}
