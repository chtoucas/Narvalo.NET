// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build.Versioning
{
    using System;
    using System.Globalization;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Narvalo.Build.Internal;
    using Narvalo.Build.Properties;

    /// <summary>
    /// MSBuild task to validate individual parts of an assembly version.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <ValidateVersionNumber Value="$(MajorVersion)" Name="MajorVersion" />
    /// <ValidateVersionNumber Value="$(MinorVersion)" Name="MinorVersion" />
    /// <ValidateVersionNumber Value="$(PatchVersion)" Name="PatchVersion" />
    /// ]]>
    /// </code>
    /// </example>
    public sealed class ValidateVersionNumber : Task
    {
        /// <summary>
        /// Gets or sets the value of the version part.
        /// </summary>
        /// <value>The value of the version part.</value>
        [Required]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the name of the version part.
        /// </summary>
        /// <value>The name of the version part.</value>
        [Required]
        public string Name { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            if (Value.Length > 1 && Value.StartsWith("0", StringComparison.OrdinalIgnoreCase))
            {
                // Semantic versioning requirement.
                Log.LogError(
                    Format.Resource(
                        Strings.ValidateVersionNumber_ValueMustNotStartWithZero_Format,
                        Name,
                        Value));
                return false;
            }

            try
            {
                // Assembly versioning requirement.
                ushort version = Convert.ToUInt16(Value, CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
                Log.LogWarning(
                    Format.Resource(
                        Strings.ValidateVersionNumber_ValueIsNotValid_Format,
                        Name,
                        Value));
                Log.LogErrorFromException(ex);
            }
            catch (OverflowException ex)
            {
                Log.LogWarning(
                    Format.Resource(
                        Strings.ValidateVersionNumber_ValueIsNotValid_Format,
                        Name,
                        Value));
                Log.LogErrorFromException(ex);
            }

            return !Log.HasLoggedErrors;
        }
    }
}
