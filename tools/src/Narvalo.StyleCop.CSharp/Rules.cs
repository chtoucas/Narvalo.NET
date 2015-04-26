// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.StyleCop.CSharp
{
    internal enum Rules
    {
        /// <summary>
        /// Private instance fields must start with an underscore.
        /// </summary>
        PrivateFieldsMustBeginWithUnderscore,

        /// <summary>
        /// Private static fields must start with "s_".
        /// </summary>
        PrivateStaticFieldsMustBeCorrectlyPrefixed,

        /// <summary>
        /// Thread static fields must start with "t_".
        /// </summary>
        ThreadStaticFieldsMustBeCorrectlyPrefixed,

        /// <summary>
        /// Private constants must only contain uppercase letters, digits and underscore.
        /// </summary>
        PrivateConstsMustOnlyContainUppercaseLettersDigitsAndUnderscores,

        /// <summary>
        /// Private methods must end with an underscore.
        /// </summary>
        PrivateMethodsMustEndWithUnderscore,

        /// <summary>
        /// Internal methods must not end with "Internal".
        /// </summary>
        InternalMethodsMustNotEndWithInternal,

        /// <summary>
        /// Private nested classes must end with an underscore.
        /// </summary>
        PrivateNestedClassesMustEndWithUnderscore,

        /// <summary>
        /// The file must start with the Narvalo copyright header:
        /// <code>
        /// // Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.
        /// </code>
        /// </summary>
        FileMustStartWithCopyrightText,

        /// <summary>
        /// Source lines should not exceed 120 characters.
        /// </summary>
        AvoidLinesExceeding120Characters,
    }
}
