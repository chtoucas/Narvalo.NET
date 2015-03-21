// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Extensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides extension methods for <see cref="Func{T}"/> that depend on the <see cref="Output{T}"/> class.
    /// </summary>
    public static partial class FuncOutputExtensions
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static Output<TSource> Catch<TSource, TException>(this Func<TSource> @this) where TException : Exception
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Output<TSource>>() != null);

            try {
                TSource value = @this.Invoke();

                return Output.Success(value);
            }
            catch (TException ex) {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return Output.Failure<TSource>(edi);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        [SuppressMessage("Microsoft.Contracts", "Suggestion-23-0",
            Justification = "[CodeContracts] Unrecognized postcondition by CCCheck.")]
        public static Output<TSource> Catch<TSource, T1Exception, T2Exception>(this Func<TSource> @this)
            where T1Exception : Exception
            where T2Exception : Exception
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Output<TSource>>() != null);

            ExceptionDispatchInfo edi;

            try {
                TSource value = @this.Invoke();

                return Output.Success(value);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return Output.Failure<TSource>(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        [SuppressMessage("Microsoft.Contracts", "Suggestion-23-0",
            Justification = "[CodeContracts] Unrecognized postcondition by CCCheck.")]
        public static Output<TSource> Catch<TSource, T1Exception, T2Exception, T3Exception>(this Func<TSource> @this)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Output<TSource>>() != null);

            ExceptionDispatchInfo edi;

            try {
                TSource value = @this.Invoke();

                return Output.Success(value);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return Output.Failure<TSource>(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        [SuppressMessage("Microsoft.Contracts", "Suggestion-23-0",
            Justification = "[CodeContracts] Unrecognized postcondition by CCCheck.")]
        public static Output<TSource> Catch<TSource, T1Exception, T2Exception, T3Exception, T4Exception>(this Func<TSource> @this)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            where T4Exception : Exception
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Output<TSource>>() != null);

            ExceptionDispatchInfo edi;

            try {
                TSource value = @this.Invoke();

                return Output.Success(value);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T4Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return Output.Failure<TSource>(edi);
        }
    }
}
