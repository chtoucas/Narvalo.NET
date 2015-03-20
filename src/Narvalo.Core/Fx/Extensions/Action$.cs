// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Extensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides extension methods for <see cref="Action"/>.
    /// </summary>
    public static class ActionExtensions
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
        public static VoidOrError Catch<TException>(this Action @this) where TException : Exception
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<VoidOrError>() != null);
            
            try
            {
                @this.Invoke();

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
        [SuppressMessage("Microsoft.Contracts", "Suggestion-23-0",
            Justification = "[CodeContracts] Unrecognized postcondition by CCCheck.")]
#endif
        public static VoidOrError Catch<T1Exception, T2Exception>(this Action @this)
            where T1Exception : Exception
            where T2Exception : Exception
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<VoidOrError>() != null);

            ExceptionDispatchInfo edi;

            try
            {
                @this.Invoke();

                return VoidOrError.Success;
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return VoidOrError.Failure(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-23-0",
            Justification = "[CodeContracts] Unrecognized postcondition by CCCheck.")]
#endif
        public static VoidOrError Catch<T1Exception, T2Exception, T3Exception>(this Action @this)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<VoidOrError>() != null);

            ExceptionDispatchInfo edi;

            try
            {
                @this.Invoke();

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
        [SuppressMessage("Microsoft.Contracts", "Suggestion-23-0",
            Justification = "[CodeContracts] Unrecognized postcondition by CCCheck.")]
#endif
        public static VoidOrError Catch<T1Exception, T2Exception, T3Exception, T4Exception>(this Action @this)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            where T4Exception : Exception
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<VoidOrError>() != null);

            ExceptionDispatchInfo edi;

            try
            {
                @this.Invoke();

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
