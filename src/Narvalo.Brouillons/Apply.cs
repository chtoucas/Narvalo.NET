// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.ExceptionServices;

    public static class Apply
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
        public static VoidOrError Catch<TException>(Action action) where TException : Exception
        {
            Require.NotNull(action, "action");
            Contract.Ensures(Contract.Result<VoidOrError>() != null);

            try
            {
                action.Invoke();

                return VoidOrError.Success;
            }
            catch (TException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return VoidOrError.Failure(edi);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[CodeContracts] Unrecognized precondition by CCCheck.")]
#endif
        public static VoidOrError Catch<T1Exception, T2Exception>(Action action)
            where T1Exception : Exception
            where T2Exception : Exception
        {
            Require.NotNull(action, "action");
            Contract.Ensures(Contract.Result<VoidOrError>() != null);

            ExceptionDispatchInfo edi;

            try
            {
                action.Invoke();

                return VoidOrError.Success;
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return VoidOrError.Failure(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[CodeContracts] Unrecognized precondition by CCCheck.")]
#endif
        public static VoidOrError Catch<T1Exception, T2Exception, T3Exception>(Action action)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
        {
            Require.NotNull(action, "action");
            Contract.Ensures(Contract.Result<VoidOrError>() != null);

            ExceptionDispatchInfo edi;

            try
            {
                action.Invoke();

                return VoidOrError.Success;
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return VoidOrError.Failure(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[CodeContracts] Unrecognized precondition by CCCheck.")]
#endif
        public static VoidOrError Catch<T1Exception, T2Exception, T3Exception, T4Exception>(Action action)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            where T4Exception : Exception
        {
            Require.NotNull(action, "action");
            Contract.Ensures(Contract.Result<VoidOrError>() != null);

            ExceptionDispatchInfo edi;

            try
            {
                action.Invoke();

                return VoidOrError.Success;
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T4Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return VoidOrError.Failure(edi);
        }
    }
}
