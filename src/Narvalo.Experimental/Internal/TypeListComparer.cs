namespace Narvalo.Internal
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
    using Narvalo.Collections;

    internal class TypeListComparer<T> : IEqualityComparer<IEnumerable<T>>
		where T : class
	{
		public bool Equals(IEnumerable<T> left, IEnumerable<T> right)
		{
			return left.SetEqual(right);
		}

		public int GetHashCode(IEnumerable<T> obj)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");

			var result = obj
				.Aggregate<T, int?>(null, (current, o) =>
					current == null ? o.GetHashCode() : current.Value | o.GetHashCode());

			return result ?? 0;
		}
	}
}