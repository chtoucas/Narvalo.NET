// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace Narvalo.TestCommon
{
    using System;
    using System.Globalization;

    internal static class My
    {
        public enum SimpleEnum
        {
            None = 0,
            ActualValue = 1,
            AliasValue = ActualValue,
        }

        [Flags]
        public enum FlagsEnum
        {
            None = 0,
            ActualValue1 = 1 << 0,
            ActualValue2 = 1 << 1,
            ActualValue3 = 1 << 2,
            CompositeValue1 = ActualValue1 | ActualValue2,
            CompositeValue2 = ActualValue1 | ActualValue2 | ActualValue3
        }

        public struct EmptyStruct { }

        public struct ComparableStruct : IComparable<ComparableStruct>
        {
            private readonly int _value;

            public ComparableStruct(int value)
            {
                _value = value;
            }

            public int CompareTo(ComparableStruct other)
            {
                return _value.CompareTo(other._value);
            }

            public override string ToString()
            {
                return _value.ToString(CultureInfo.CurrentCulture);
            }
        }

        public sealed class SimpleValue
        {
            private readonly int _value;

            public SimpleValue(int value)
            {
                _value = value;
            }

            public int Value { get { return _value; } }
        }
    }
}
