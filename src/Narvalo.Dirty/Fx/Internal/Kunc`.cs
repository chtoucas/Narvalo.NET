namespace Narvalo.Fx.Internal
{
    using System.Diagnostics.CodeAnalysis;

    delegate Monad<T> Kunc<T>();

    delegate Monad<TResult> Kunc<T, TResult>(T arg);

    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    delegate Monad<TResult> Kunc<T1, T2, TResult>(T1 arg1, T2 arg2);
}
