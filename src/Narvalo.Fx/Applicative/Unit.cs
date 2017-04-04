// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public struct Unit : IEquatable<Unit>, IEquatable<ValueTuple>
    {
        private static readonly Unit s_Default = new Unit();

        public static Unit Default => s_Default;

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left", Justification = "[Intentionally] This method always returns 'true'.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right", Justification = "[Intentionally] This method always returns 'true'.")]
        public static bool operator ==(Unit left, Unit right) => true;

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left", Justification = "[Intentionally] This method always returns 'false'.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right", Justification = "[Intentionally] This method always returns 'false'.")]
        public static bool operator !=(Unit left, Unit right) => false;

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left", Justification = "[Intentionally] This method always returns 'true'.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right", Justification = "[Intentionally] This method always returns 'true'.")]
        public static bool operator ==(Unit left, ValueTuple right) => true;

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left", Justification = "[Intentionally] This method always returns 'false'.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right", Justification = "[Intentionally] This method always returns 'false'.")]
        public static bool operator !=(Unit left, ValueTuple right) => false;

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left", Justification = "[Intentionally] This method always returns 'true'.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right", Justification = "[Intentionally] This method always returns 'true'.")]
        public static bool operator ==(ValueTuple left, Unit right) => true;

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left", Justification = "[Intentionally] This method always returns 'false'.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right", Justification = "[Intentionally] This method always returns 'false'.")]
        public static bool operator !=(ValueTuple left, Unit right) => false;

        public bool Equals(Unit other) => true;

        public bool Equals(ValueTuple other) => true;

        public override bool Equals(object obj) => obj is Unit || obj is ValueTuple;

        public override int GetHashCode() => 0;

        public override string ToString() => "()";
    }
}
