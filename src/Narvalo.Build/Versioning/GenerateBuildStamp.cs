// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build.Versioning
{
    using System;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// MSBuild task to generate a unique stamp from build and revision numbers.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <GenerateBuildStamp BuildNumber="$(BuildNumber)" RevisionNumber="$(RevisionNumber)">
    ///     <Output TaskParameter="BuildStamp" PropertyName="BuildStamp"/>
    /// </GenerateBuildStamp>
    /// ]]>
    /// </code>
    /// </example>
    public sealed class GenerateBuildStamp : Task
    {
        /// <summary>
        /// Gets or sets the build number.
        /// </summary>
        /// <value>The build number.</value>
        [Required]
        [CLSCompliant(false)]
        public ushort BuildNumber { get; set; }

        /// <summary>
        /// Gets or sets the revision number.
        /// </summary>
        /// <value>The revision number.</value>
        [Required]
        [CLSCompliant(false)]
        public ushort RevisionNumber { get; set; }

        /// <summary>
        /// Gets or sets the unique stamp.
        /// </summary>
        /// <value>The unique stamp.</value>
        [Output]
        public string BuildStamp { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns><see langword="true"/> if the task successfully executed; otherwise, 
        /// <see langword="false"/>.</returns>
        public override bool Execute()
        {
            // BuildNumber + "0...0" + RevisionNumber.
            var stamp = BuildNumber.ToString();

            var revisionString = RevisionNumber.ToString();

            for (int i = 0; i < 5 - revisionString.Length; i++) { stamp += "0"; }

            stamp += revisionString;

            BuildStamp = stamp;

            return !Log.HasLoggedErrors;
        }
    }
}
