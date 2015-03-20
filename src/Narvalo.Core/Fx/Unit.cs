// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    public struct Unit : IEquatable<Unit>
    {
        public static readonly Unit Single = new Unit();

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left",
            Justification = "[Ignore] This method always returns the same result.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right",
            Justification = "[Ignore] This method always returns the same result.")]
        public static bool operator ==(Unit left, Unit right)
        {
            Contract.Ensures(Contract.Result<bool>() == true);

            return true;
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left",
            Justification = "[Ignore] This method always returns the same result.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right",
            Justification = "[Ignore] This method always returns the same result.")]
        public static bool operator !=(Unit left, Unit right)
        {
            Contract.Ensures(Contract.Result<bool>() == false);

            return false;
        }

        public bool Equals(Unit other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Unit;
        }

        public override int GetHashCode()
        {
            Contract.Ensures(Contract.Result<int>() == 0);

            return 0;
        }

        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return "()";
        }
    }
}
