﻿namespace Narvalo.Fx
{
    using System;

    public static class Failure
    {
        //// Create

        public static Failure<T> Create<T>(T exception) where T : Exception
        {
            return Failure<T>.η(exception);
        }
    }
}
