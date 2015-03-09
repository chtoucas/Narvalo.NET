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
            Justification = "This method always returns the same result.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right",
            Justification = "This method always returns the same result.")]
        public static bool operator ==(Unit left, Unit right)
        {
            return true;
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left",
            Justification = "This method always returns the same result.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right",
            Justification = "This method always returns the same result.")]
#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-1-0",
            Justification = "[CodeContracts] We do not want to enforce the contract here.")]
#endif
        public static bool operator !=(Unit left, Unit right)
        {
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

#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-1-0",
            Justification = "[CodeContracts] We do not want to enforce the contract here.")]
#endif
        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return "()";
        }
    }
}
