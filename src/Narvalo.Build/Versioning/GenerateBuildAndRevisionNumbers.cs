// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build.Versioning
{
    using System;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// MSBuild task to generate build and revision numbers.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <GenerateBuildAndRevisionNumbers>
    ///     <Output TaskParameter="BuildNumber" PropertyName="BuildNumber"/>
    ///     <Output TaskParameter="RevisionNumber" PropertyName="RevisionNumber"/>
    /// </GenerateBuildAndRevisionNumbers>
    /// ]]>
    /// </code>
    /// </example>
    /**
     * <content markup="commonmark">
     * <![CDATA[
     * The algorithm used by .NET to auto-increment the assembly version is as follows:
     * The default build number increments daily. The default revision number is
     * the number of seconds since midnight local time (without taking into account
     * time zone adjustments for daylight saving time), divided by 2.
     * For further informations, see
     * [AssemblyVersionAttribute](http://msdn.microsoft.com/en-us/library/system.reflection.assemblyversionattribute.aspx).
     *
     * Remarks:
     * - This feature is only available to `AssemblyVersion`, not to `AssemblyFileVersion`.
     * - During the same day, two builds may end up with the same assembly version.
     * - Build and revision numbers must be less or equal to 65534 (ie `UInt16.MaxValue - 1`).
     *
     * Here I implement a slight variation of the same algorithm:
     * - Use UTC time.
     * - The build number is the number of half-days since 2014-12-01 (minus one to start at zero).
     * - The revision number is the number of seconds since midnight in the morning
     *   and since midday in the afternoon.
     *
     * This way, there is less chance of getting the same numbers during
     * a single day on the same build machine. The scheme will break in approximately 89 years...
     *
     * Worth reminding, if we used a scheme that simply incremented the build
     * numbers, we would generate a lot of unecessary holes in the sequence.
     * Indeed due to incremental batching a build might not do anything.
     * That's a good reason to use an algorithm that only depends on the date and the time.
     * ]]>
     * </content>
     */
    public sealed class GenerateBuildAndRevisionNumbers : Task
    {
        /// <summary>
        /// Gets the build number.
        /// </summary>
        /// <value>The build number.</value>
        [Output]
        [CLSCompliant(false)]
        public ushort BuildNumber { get; private set; }

        /// <summary>
        /// Gets the revision number.
        /// </summary>
        /// <value>The revision number.</value>
        [Output]
        [CLSCompliant(false)]
        public ushort RevisionNumber { get; private set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            var now = DateTime.UtcNow;
            var isMorning = now.Hour < 12;
            var numberOfHalfDays = 2 * (now - new DateTime(2014, 11, 30)).Days - (isMorning ? 1 : 0);
            var numberOfSeconds =
                (now - new DateTime(now.Year, now.Month, now.Day, isMorning ? 0 : 12, 0, 0)).TotalSeconds;

            BuildNumber = (ushort)(numberOfHalfDays - 1);
            RevisionNumber = (ushort)numberOfSeconds;

            return !Log.HasLoggedErrors;
        }
    }
}
