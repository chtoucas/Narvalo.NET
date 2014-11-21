namespace Narvalo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using Narvalo.Fx;

    public static partial class FuncExtensions
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
        public static Output<TResult> Catch<TException, TResult>(this Func<TResult> @this)
            where TException : Exception
        {
            Contract.Requires(@this != null); 
            
            return Make<TResult>.Catch<TException>(@this);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
        public static Output<TResult> Catch<T1Exception, T2Exception, TResult>(this Func<TResult> @this)
            where T1Exception : Exception
            where T2Exception : Exception
        {
            Contract.Requires(@this != null); 
            
            return Make<TResult>.Catch<T1Exception, T2Exception>(@this);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
        public static Output<TResult> Catch<T1Exception, T2Exception, T3Exception, TResult>(this Func<TResult> @this)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
        {
            Contract.Requires(@this != null); 
            
            return Make<TResult>.Catch<T1Exception, T2Exception, T3Exception>(@this);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
        public static Output<TResult> Catch<T1Exception, T2Exception, T3Exception, T4Exception, TResult>(this Func<TResult> @this)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            where T4Exception : Exception
        {
            Contract.Requires(@this != null); 
            
            return Make<TResult>.Catch<T1Exception, T2Exception, T3Exception, T4Exception>(@this);
        }
    }
}
