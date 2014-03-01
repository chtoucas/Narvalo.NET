// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    public partial class Output<T>
    {
        #region Monad

        public Output<TResult> Bind<TResult>(Func<T, Output<TResult>> selector)
        {
            Require.NotNull(selector, "selector");

            return IsFailure ? Output<TResult>.η(ExceptionInfo) : selector.Invoke(Value);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics.")]
        internal static Output<T> η(ExceptionDispatchInfo exceptionInfo)
        {
            Require.NotNull(exceptionInfo, "exceptionInfo");

            return new Output<T>(exceptionInfo);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics.")]
        internal static Output<T> η(T value)
        {
            return new Output<T>(value);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics.")]
        internal static Output<T> μ(Output<Output<T>> square)
        {
            Require.NotNull(square, "square");

            return square.IsSuccess ? square.Value : η(square.ExceptionInfo);
        }

        #endregion

        #region Basic Monad functions

        public Output<TResult> Map<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, "selector");

            return IsFailure ? Output<TResult>.η(ExceptionInfo) : Output<TResult>.η(selector.Invoke(Value));
        }

        public Output<TResult> Then<TResult>(Output<TResult> other)
        {
            return IsFailure ? Output<TResult>.η(ExceptionInfo) : other;
        }

        #endregion

        #region Non-standard extensions

        public Output<T> Run(Action<T> action)
        {
            Require.NotNull(action, "action");

            if (IsSuccess) {
                action.Invoke(Value);
            }

            return this;
        }

        #endregion

        public Output<T> OnSuccess(Action<T> action)
        {
            return Run(action);
        }

        public Output<T> OnFailure(Action<ExceptionDispatchInfo> action)
        {
            Require.NotNull(action, "action");

            if (IsFailure) {
                action.Invoke(ExceptionInfo);
            }

            return this;
        }
    }
}
