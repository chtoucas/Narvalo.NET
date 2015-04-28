// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Comparisons
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Reflection.Emit;

    using Narvalo.BenchmarkCommon;

    [BenchmarkComparison(1000, DisplayName = "Appel dynamique du constructeur par défaut.")]
    public static partial class EmptyCtorComparison
    {
        public class MyDerived : MyBase
        {
            private static readonly MyDerived s_Instance = new MyDerived();

            private MyDerived() : base() { }
        }

        public class MyBase
        {
            internal MyBase() { }
        }
    }

    public static partial class EmptyCtorComparison
    {
        [BenchmarkComparative(DisplayName = "Activator")]
        public static void ViaActivator()
        {
            var inst = NewActivator<MyDerived>();
        }

        [BenchmarkComparative(DisplayName = "Dynamic method")]
        public static void ViaDynamicMethod()
        {
            var inst = NewDynamicMethod<MyDerived>();
        }

        [BenchmarkComparative(DisplayName = "Expression")]
        public static void ViaExpression()
        {
            var inst = NewExpression<MyDerived>();
        }

        [BenchmarkComparative(DisplayName = "Static property")]
        public static void ViaUniqueInstance()
        {
            var inst = NewStaticField<MyDerived>();
        }

        private static Func<T> NewActivator<T>() where T : class
        {
            return () => Activator.CreateInstance(typeof(T), nonPublic: true) as T;
        }

        private static Func<T> NewStaticField<T>()
        {
            var type = typeof(T);
            var fieldInfo = type.GetField("s_Instance", BindingFlags.NonPublic | BindingFlags.Static);
            var fieldValue = fieldInfo.GetValue(null);

            return () => (T)fieldValue;
        }

        private static Func<T> NewDynamicMethod<T>() where T : MyBase
        {
            var type = typeof(T);
            var ctorInfo = type.GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);
            var method = new DynamicMethod("NewTCurrency", type, Type.EmptyTypes, type, skipVisibility: true);

            var il = method.GetILGenerator();
            il.Emit(OpCodes.Newobj, ctorInfo);
            il.Emit(OpCodes.Ret);

            return (Func<T>)method.CreateDelegate(typeof(Func<T>));
        }

        private static Func<T> NewExpression<T>() where T : MyBase
        {
            var expr = Expression.New(typeof(T));

            return Expression.Lambda<Func<T>>(expr).Compile();
        }
    }
}
