namespace Narvalo.Fx.Theory
{
    using System.Diagnostics.CodeAnalysis;

    public delegate Monad<T> Kunc<T>();

    public delegate Monad<TResult> Kunc<T, TResult>(T arg);

    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public delegate Monad<TResult> Kunc<T1, T2, TResult>(T1 arg1, T2 arg2);
}
