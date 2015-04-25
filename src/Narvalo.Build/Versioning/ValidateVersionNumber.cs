// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build.Versioning
{
    using System;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

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

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns><see langword="true"/> if the task successfully executed; otherwise, 
        /// <see langword="false"/>.</returns>
        public override bool Execute()
        {
            if (Value.Length > 1 && Value.StartsWith("0"))
            {
                // Semantic versioning requirement.
                Log.LogError(Name + " (" + Value + ") MUST NOT contain leading zeroes.");
                return false;
            }

            try
            {
                // Assembly versioning requirement.
                ushort version = Convert.ToUInt16(Value);
            }
            catch (FormatException ex)
            {
                Log.LogWarning(Name + " (" + Value + ") MUST be a 16-bit unsigned integers.");
                Log.LogErrorFromException(ex);
            }
            catch (OverflowException ex)
            {
                Log.LogWarning(Name + " (" + Value + ") MUST be a 16-bit unsigned integers.");
                Log.LogErrorFromException(ex);
            }

            return !Log.HasLoggedErrors;
        }
    }
}
