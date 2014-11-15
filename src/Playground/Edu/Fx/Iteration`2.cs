// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Edu.Fx
{
    using System;

    public class Iteration<TResult, TSource>
    {
        readonly Tuple<TResult, TSource> _pair;

        public Iteration(Tuple<TResult, TSource> pair)
        {
            _pair = pair;
        }

        public TResult Result { get { return _pair.Item1; } }
        public TSource Next { get { return _pair.Item2; } }
    }
}
