// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build.Versioning
{
    using System;
    using System.Globalization;

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
        /// Gets the unique stamp.
        /// </summary>
        /// <value>The unique stamp.</value>
        [Output]
        public string BuildStamp { get; private set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            // BuildNumber + "0...0" + RevisionNumber.
            var stamp = BuildNumber.ToString(CultureInfo.InvariantCulture);

            var revisionString = RevisionNumber.ToString(CultureInfo.InvariantCulture);

            for (int i = 0; i < 5 - revisionString.Length; i++) { stamp += "0"; }

            stamp += revisionString;

            BuildStamp = stamp;

            return !Log.HasLoggedErrors;
        }
    }
}
