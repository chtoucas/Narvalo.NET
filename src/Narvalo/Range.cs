namespace Narvalo
{
    using System;

    public static class Range
    {
        public static Range<T> Create<T>(T lowerEnd, T upperEnd)
            where T : IEquatable<T>, IComparable<T>
        {
            return new Range<T>(lowerEnd, upperEnd);
        }

        public static Range<DateTime> OneDay(int year, int month, int day)
        {
            var lowerEnd = new DateTime(year, month, day);
            var upperEnd = lowerEnd.AddHours(23).AddMinutes(59).AddSeconds(59);

            return Create(lowerEnd, upperEnd);
        }
    }
}

