// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build.Versioning
{
    using System.Text.RegularExpressions;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Narvalo.Build.Internal;
    using Narvalo.Build.Properties;

    /// <summary>
    /// MSBuild task to validate the pre-release label for an assembly informational version.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <ValidatePreReleaseLabel Value="$(PreReleaseLabel)" Condition=" '$(PreReleaseLabel)' != '' " />
    /// ]]>
    /// </code>
    /// </example>
    public sealed class ValidatePrereleaseLabel : Task
    {
        /// <summary>
        /// Gets or sets the pre-release label.
        /// </summary>
        /// <value>The pre-release label.</value>
        [Required]
        public string Value { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            if (!Regex.IsMatch(Value, @"^[a-z][\-0-9a-z]*$", RegexOptions.IgnoreCase))
            {
                Log.LogError(
                    Format.Resource(
                        Strings.ValidatePrereleaseLabel_ValueIsNotValid_Format,
                        Value));
            }

            return !Log.HasLoggedErrors;
        }
    }
}
