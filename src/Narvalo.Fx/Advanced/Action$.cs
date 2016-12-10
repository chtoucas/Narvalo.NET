// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Advanced
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    using static System.Diagnostics.Contracts.Contract;

    /// <summary>
    /// Provides extension methods for <see cref="Action"/>.
    /// </summary>
    public static class ActionExtensions
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static VoidOrError Catch<TException>(this Action @this) where TException : Exception
        {
            Require.NotNull(@this, nameof(@this));
            Ensures(Result<VoidOrError>() != null);

            try
            {
                @this.Invoke();

                return VoidOrError.Void;
            }
            catch (TException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return VoidOrError.Error(edi);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        [SuppressMessage("Microsoft.Contracts", "Suggestion-20-0",
            Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public static VoidOrError Catch<T1Exception, T2Exception>(this Action @this)
            where T1Exception : Exception
            where T2Exception : Exception
        {
            Require.NotNull(@this, nameof(@this));
            Ensures(Result<VoidOrError>() != null);

            ExceptionDispatchInfo edi;

            try
            {
                @this.Invoke();

                return VoidOrError.Void;
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return VoidOrError.Error(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        [SuppressMessage("Microsoft.Contracts", "Suggestion-20-0",
            Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public static VoidOrError Catch<T1Exception, T2Exception, T3Exception>(this Action @this)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
        {
            Require.NotNull(@this, nameof(@this));
            Ensures(Result<VoidOrError>() != null);

            ExceptionDispatchInfo edi;

            try
            {
                @this.Invoke();

                return VoidOrError.Void;
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return VoidOrError.Error(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        [SuppressMessage("Microsoft.Contracts", "Suggestion-20-0",
            Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public static VoidOrError Catch<T1Exception, T2Exception, T3Exception, T4Exception>(this Action @this)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            where T4Exception : Exception
        {
            Require.NotNull(@this, nameof(@this));
            Ensures(Result<VoidOrError>() != null);

            ExceptionDispatchInfo edi;

            try
            {
                @this.Invoke();

                return VoidOrError.Void;
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T4Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return VoidOrError.Error(edi);
        }
    }
}
