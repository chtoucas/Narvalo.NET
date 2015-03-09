// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.ExceptionServices;

    public static class Make<T>
    {
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "A non-generic version would not improve usability.")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
        public static Output<T> Catch<TException>(Func<T> valueFactory) where TException : Exception
        {
            Require.NotNull(valueFactory, "valueFactory");
            Contract.Ensures(Contract.Result<Output<T>>() != null);

            try {
                T value = valueFactory.Invoke();

                return Output.Success(value);
            }
            catch (TException ex) {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return Output.Failure<T>(edi);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "A non-generic version would not improve usability.")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[CodeContracts] Unrecognized precondition by CCCheck.")]
#endif
        public static Output<T> Catch<T1Exception, T2Exception>(Func<T> valueFactory)
            where T1Exception : Exception
            where T2Exception : Exception
        {
            Require.NotNull(valueFactory, "valueFactory");
            Contract.Ensures(Contract.Result<Output<T>>() != null);

            ExceptionDispatchInfo edi;

            try {
                T value = valueFactory.Invoke();

                return Output.Success(value);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return Output.Failure<T>(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "A non-generic version would not improve usability.")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[CodeContracts] Unrecognized precondition by CCCheck.")]
#endif
        public static Output<T> Catch<T1Exception, T2Exception, T3Exception>(Func<T> valueFactory)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
        {
            Require.NotNull(valueFactory, "valueFactory");
            Contract.Ensures(Contract.Result<Output<T>>() != null);

            ExceptionDispatchInfo edi;

            try {
                T value = valueFactory.Invoke();

                return Output.Success(value);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return Output.Failure<T>(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "A non-generic version would not improve usability.")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[CodeContracts] Unrecognized precondition by CCCheck.")]
#endif
        public static Output<T> Catch<T1Exception, T2Exception, T3Exception, T4Exception>(Func<T> valueFactory)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            where T4Exception : Exception
        {
            Require.NotNull(valueFactory, "valueFactory");
            Contract.Ensures(Contract.Result<Output<T>>() != null);

            ExceptionDispatchInfo edi;

            try {
                T value = valueFactory.Invoke();

                return Output.Success(value);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T4Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return Output.Failure<T>(edi);
        }
    }
}
