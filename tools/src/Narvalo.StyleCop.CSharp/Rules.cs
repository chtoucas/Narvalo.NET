// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.StyleCop.CSharp
{
    internal enum Rules
    {
        /// <summary>
        /// Names of private instance fields must start with an underscore.
        /// </summary>
        PrivateFieldNamesMustBeginWithUnderscore,

        /// <summary>
        /// Names of private methods must end with an underscore.
        /// </summary>
        PrivateMethodNamesMustEndWithUnderscore,

        /// <summary>
        /// Names of private static fields must be start with "s_".
        /// </summary>
        PrivateStaticFieldMustBePrefixed,

        /// <summary>
        /// Names of private constants must only contain uppercase letters and underscore.
        /// </summary>
        PrivateConstantNamesMustOnlyContainUppercaseLettersAndUnderscore,
    }
}
