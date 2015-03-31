// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    /// <summary>
    /// Decorating a parameter with this attribute informs the Code Analysis tool
    /// that the method is validating the parameter against <see langword="null"/> value.
    /// </summary>
    /// <remarks>
    /// Using this attribute suppresses the CA1062 warning.
    /// </remarks>
    /// <seealso cref="!:http://geekswithblogs.net/terje/archive/2010/10/14/making-static-code-analysis-and-code-contracts-work-together-or.aspx" />
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class ValidatedNotNullAttribute : Attribute { }
}
