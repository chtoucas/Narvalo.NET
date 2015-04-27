// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    internal enum RuleName
    {
        #region Naming Rules

        /// <summary>
        /// Private instance field names must start with an underscore.
        /// </summary>
        PrivateFieldNamesMustBeginWithUnderscore,

        /// <summary>
        /// Private static field names must start with "s_".
        /// </summary>
        PrivateStaticFieldNamesMustBeCorrectlyPrefixed,

        /// <summary>
        /// Thread static field names must start with "t_".
        /// </summary>
        ThreadStaticFieldNamesMustBeCorrectlyPrefixed,

        /// <summary>
        /// Private constant names must only contain uppercase letters, digits and underscores.
        /// </summary>
        PrivateConstNamesMustOnlyContainUppercaseLettersDigitsAndUnderscores,

        /// <summary>
        /// Private method names must end with an underscore.
        /// </summary>
        PrivateMethodNamesMustEndWithUnderscore,

        /// <summary>
        /// Internal method names must not end with "Internal".
        /// </summary>
        InternalMethodNamesMustNotEndWithInternal,

        /// <summary>
        /// Private class names must end with an underscore.
        /// </summary>
        PrivateClassNamesMustEndWithUnderscore,

        /// <summary>
        /// Abstract class names must not end with "Base".
        /// </summary>
        AbstractClassNamesMustNotEndWithBase,

        /// <summary>
        /// File names must match type names.
        /// </summary>
        FileNamesMustMatchTypeNames,

        /// <summary>
        /// File names must match extension class names followed by a dollar sign.
        /// </summary>
        FileNamesMustMatchExtensionClassNamesFollowedByDollarSign,

        #endregion

        #region Readibility Rules

        /// <summary>
        /// Source lines should not exceed 120 characters.
        /// </summary>
        AvoidLinesExceeding120Characters,

        ConstructorsMustNotUseBuiltInAliases,

        StaticMethodsMustNotUseBuiltInAliases,

        TypeOfMustNotUseBuiltInAliases,

        UsingDirectivesMustBeSortedAlphabeticallyByGroup,

        UsingDirectiveGroupsMustFollowGivenOrder,

        FirstUsingDirectivesMustBeSystem,

        UsingDirectiveGroupsMustBeSeparatedByBlankLine,

        #endregion

        #region Documentation Rules

        /// <summary>
        /// Files must start with the Narvalo copyright header:
        /// <code>
        /// // Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.
        /// </code>
        /// </summary>
        FilesMustStartWithCopyrightText,

        #endregion

        #region Maintainability Rules

        DirectoriesMustMirrorNamespaces,

        #endregion

        #region Spacing Rules

        /// <summary>
        /// Source lines should not have any trailing whitespaces.
        /// </summary>
        AvoidLinesWithTrailingWhiteSpaces,

        #endregion
    }
}
