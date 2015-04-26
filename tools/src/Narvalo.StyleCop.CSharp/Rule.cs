// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    internal enum Rule
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
        /// Private constants must only contain uppercase letters, digits and underscores.
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
        /// Abstract classes must not end with "Base".
        /// </summary>
        AbstractClassesMustNotEndWithBase,

        /// <summary>
        /// Private classes must end with an underscore.
        /// </summary>
        PrivateClassesMustEndWithUnderscore,

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
