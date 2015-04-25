// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build.Versioning
{
    using System.Text.RegularExpressions;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

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
    public sealed class ValidatePreReleaseLabel : Task
    {
        /// <summary>
        /// Gets or sets the pre-release label.
        /// </summary>
        /// <value>The pre-release label.</value>
        [Required]
        public string Value { get; set; }
        
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns><see langword="true"/> if the task successfully executed; otherwise, 
        /// <see langword="false"/>.</returns>
        public override bool Execute()
        {
            if (!Regex.IsMatch(Value, @"^[a-z][\-0-9a-z]*$", RegexOptions.IgnoreCase))
            {
                Log.LogError("PreReleaseLabel (" + Value + ") MUST comprise only ASCII alphanumerics and hyphen. It MUST also start with an ASCII letter.");
            }

            return !Log.HasLoggedErrors;
        }
    }
}
