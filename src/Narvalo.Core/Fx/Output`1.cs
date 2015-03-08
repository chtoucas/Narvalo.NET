// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.ExceptionServices;

    public sealed partial class Output<T>
    {
        private readonly bool _isSuccess;
        private readonly ExceptionDispatchInfo _exceptionInfo;
        private readonly T _value;

        private Output(ExceptionDispatchInfo exceptionInfo)
        {
            Require.NotNull(exceptionInfo, "exceptionInfo");

            _isSuccess = false;
            _exceptionInfo = exceptionInfo;
        }

        private Output(T value)
        {
            _isSuccess = true;
            _value = value;
        }

        internal bool IsFailure { get { return !_isSuccess; } }

        internal bool IsSuccess { get { return _isSuccess; } }

        internal ExceptionDispatchInfo ExceptionInfo
        {
            get
            {
                Contract.Ensures(Contract.Result<ExceptionDispatchInfo>() != null);

                if (_isSuccess)
                {
                    throw new InvalidOperationException(Strings_Core.Output_SuccessfulHasNoException);
                }

                return _exceptionInfo;
            }
        }

        internal T Value
        {
            get
            {
                if (!_isSuccess)
                {
                    throw new InvalidOperationException(Strings_Core.Output_UnsuccessfulHasNoValue);
                }

                return _value;
            }
        }

#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-30-0",
            Justification = "[CodeContracts] Unrecognized fix by CCCheck.")]
#endif
        public Output<T> OnSuccess(Action<T> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<Output<T>>() != null);

            return Run(action);
        }

        public Output<T> OnFailure(Action<ExceptionDispatchInfo> action)
        {
            Require.NotNull(action, "action");
            Contract.Ensures(Contract.Result<Output<T>>() != null);

            if (IsFailure)
            {
                action.Invoke(ExceptionInfo);
            }

            return this;
        }

        /// <summary>
        /// Obtains the underlying value if any; otherwise the default value of the type T.
        /// </summary>
        /// <returns>The underlying value if any; otherwise the default value of the type T.</returns>
        public T ValueOrDefault()
        {
            return _isSuccess ? _value : default(T);
        }

        /// <summary>
        /// Returns the underlying value if any; otherwise <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="defaultValue">A default value to be used if if there is no underlying value.</param>
        /// <returns>The underlying value if any; otherwise <paramref name="defaultValue"/>.</returns>
        public T ValueOrElse(T defaultValue)
        {
            return _isSuccess ? _value : defaultValue;
        }

        public T ValueOrElse(Func<T> defaultValueFactory)
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return _isSuccess ? _value : defaultValueFactory.Invoke();
        }

        public T ValueOrThrow()
        {
            if (!_isSuccess)
            {
                _exceptionInfo.Throw();
            }

            return Value;
        }

        public override string ToString()
        {
            return _isSuccess ? Value.ToString() : _exceptionInfo.ToString();
        }

#if CONTRACTS_FULL

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(IsSuccess == !IsFailure);
            Contract.Invariant(IsSuccess || ExceptionInfo != null);
        }

#endif
    }

    // Monad definition.
    public partial class Output<T>
    {
        public Output<TResult> Bind<TResult>(Func<T, Output<TResult>> selector)
        {
            Require.NotNull(selector, "selector");

            // FIXME: Incorrect? We should catch exceptions.
            return IsFailure ? Output<TResult>.η(ExceptionInfo) : selector.Invoke(Value);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics. Only used internally.")]
        internal static Output<T> η(ExceptionDispatchInfo exceptionInfo)
        {
            Require.NotNull(exceptionInfo, "exceptionInfo");
            Contract.Ensures(Contract.Result<Output<T>>() != null);

            return new Output<T>(exceptionInfo);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics. Only used internally.")]
        internal static Output<T> η(T value)
        {
            Contract.Ensures(Contract.Result<Output<T>>() != null);

            return new Output<T>(value);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics. Only used internally.")]
        internal static Output<T> μ(Output<Output<T>> square)
        {
            Require.NotNull(square, "square");
            Assume.Invariant(square);

            return square.IsSuccess ? square.Value : η(square.ExceptionInfo);
        }
    }

    // Monad optimized extensions.
    public partial class Output<T>
    {
        #region Basic Monad functions

#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[CodeContracts] Unrecognized fix by CCCheck.")]
#endif
        public Output<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, "selector");
            Contract.Ensures(Contract.Result<Output<TResult>>() != null);

            // FIXME: Incorrect? Should we catch exceptions?
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
            Contract.Ensures(Contract.Result<Output<T>>() != null);

            if (IsSuccess)
            {
                // FIXME: Incorrect? We should catch exceptions?
                action.Invoke(Value);
            }

            return this;
        }

        #endregion
    }
}
