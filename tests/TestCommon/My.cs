// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    public static partial class My
    {
        // To be used only when we need a strongly-typed null-string.
        public const string NullString = null;

        public const string WhiteSpaceOnlyString = "     ";

        public enum Enum012
        {
            Zero = 0,
            One = 1,
            Two = 2,
            Alias1 = One,
        }

        [Flags]
        public enum EnumBits
        {
            Zero = 0,
            One = 1 << 0,
            Two = 1 << 1,
            Four = 1 << 2,
            OneTwo = One | Two,
            OneTwoFour = One | Two | Four
        }

        public struct EmptyStruct { }

        public struct ComparableStruct : IComparable<ComparableStruct>
        {
            private readonly int _value;

            public ComparableStruct(int value)
            {
                _value = value;
            }

            public int CompareTo(ComparableStruct other) => _value.CompareTo(other._value);

            public override string ToString() => _value.ToString(CultureInfo.CurrentCulture);
        }

        [Serializable]
        public sealed class SimpleException : Exception
        {
            public SimpleException() : base() { }

            public SimpleException(string message) : base(message) { }

            public SimpleException(string message, Exception innerException)
                : base(message, innerException) { }

            private SimpleException(SerializationInfo info, StreamingContext context)
                : base(info, context) { }
        }
    }
}
