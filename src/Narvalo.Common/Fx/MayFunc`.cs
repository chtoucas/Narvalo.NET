namespace Narvalo.Fx
{
    using System.Diagnostics.CodeAnalysis;

    public delegate Maybe<T> MayFunc<T>();

    public delegate Maybe<TResult> MayFunc<T, TResult>(T arg);

    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public delegate Maybe<TResult> MayFunc<T1, T2, TResult>(T1 arg1, T2 arg2);
}
