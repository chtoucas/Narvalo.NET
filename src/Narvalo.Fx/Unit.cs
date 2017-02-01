// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using static System.Diagnostics.Contracts.Contract;

    public struct Unit : IEquatable<Unit>
    {
        public static readonly Unit Single = new Unit();

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left", Justification = "[Intentionally] This method always returns 'true'.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right", Justification = "[Intentionally] This method always returns 'true'.")]
        public static bool operator ==(Unit left, Unit right)
        {
            Warrant.IsTrue();

            return true;
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left", Justification = "[Intentionally] This method always returns 'false'.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right", Justification = "[Intentionally] This method always returns 'false'.")]
        public static bool operator !=(Unit left, Unit right)
        {
            Warrant.IsFalse();

            return false;
        }

        public bool Equals(Unit other) => true;

        public override bool Equals(object obj) => obj is Unit;

        public override int GetHashCode()
        {
            Ensures(Result<int>() == 0);

            return 0;
        }

        public override string ToString()
        {
            Warrant.NotNull<string>();

            return "()";
        }
    }
}
