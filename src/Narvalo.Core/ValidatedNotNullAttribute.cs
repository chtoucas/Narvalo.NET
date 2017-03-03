// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    /// <summary>
    /// Decorating a parameter with this attribute informs the Code Analysis tool
    /// that the method is validating the parameter against null value.
    /// </summary>
    /// <remarks>Using this attribute suppresses the CA1062 warning.</remarks>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class ValidatedNotNullAttribute : Attribute { }
}
