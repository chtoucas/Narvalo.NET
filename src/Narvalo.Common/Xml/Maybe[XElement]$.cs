﻿namespace Narvalo.Xml
{
    using System;
    using System.Xml.Linq;
    using Narvalo.Fx;

    /// <summary>
    /// Fournit des méthodes d'extension pour Maybe&lt;XElement&gt;.
    /// </summary>
    public static class MaybeXElementExtensions
    {
        public static Maybe<T> BindValue<T>(this Maybe<XElement> @this, Func<string, Maybe<T>> fun)
        {
            return @this.Bind(_ => fun.Invoke(_.Value));
        }

        public static Maybe<T> MapValue<T>(this Maybe<XElement> @this, Func<string, T> fun)
        {
            return @this.Map(_ => fun.Invoke(_.Value));
        }

        public static Maybe<string> ValueOrNone(this Maybe<XElement> @this)
        {
            return @this.MapValue(_ => _);
        }
    }
}
