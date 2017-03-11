// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using Narvalo.Mvp.Properties;

    public sealed partial class CompositeViewTypeBuilder
    {
        private static readonly MethodAttributes s_MethodAttributes
               = MethodAttributes.Public
               | MethodAttributes.SpecialName
               | MethodAttributes.HideBySig
               | MethodAttributes.Virtual;

        private readonly TypeBuilder _typeBuilder;
        private readonly Type _viewType;

        private Type _compositeViewType;
        private Type _enumerableType;
        private Type _enumeratorType;

        public CompositeViewTypeBuilder(Type viewType, TypeBuilder typeBuilder)
        {
            Require.NotNull(viewType, nameof(viewType));
            Require.NotNull(typeBuilder, nameof(typeBuilder));

            _viewType = viewType;
            _typeBuilder = typeBuilder;
        }

        private Type CompositeViewType
        {
            get
            {
                if (_compositeViewType == null)
                {
                    var type = typeof(CompositeView<>);

                    _compositeViewType = type.MakeGenericType(new Type[] { _viewType });
                }

                return _compositeViewType;
            }
        }

        private Type EnumerableType
        {
            get
            {
                if (_enumerableType == null)
                {
                    var type = typeof(IEnumerable<>);

                    _enumerableType = type.MakeGenericType(new Type[] { _viewType });
                }

                return _enumerableType;
            }
        }

        private Type EnumeratorType
        {
            get
            {
                if (_enumeratorType == null)
                {
                    var type = typeof(IEnumerator<>);

                    _enumeratorType = type.MakeGenericType(new Type[] { _viewType });
                }

                return _enumeratorType;
            }
        }

        public Type Build() => _typeBuilder.CreateType();

        public void AddEvent(EventInfo eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));

            if (eventInfo.EventHandlerType == null)
            {
                throw new ArgumentException(Format.Current(
                        Strings.CompositeViewTypeBuilder_EventHandlerTypeMismatch,
                        eventInfo.Name,
                        eventInfo.ReflectedType.Name),
                    nameof(eventInfo));
            }

            var addMethod = DefineAddMethod(eventInfo);
            var removeMethod = DefineRemoveMethod(eventInfo);

            var @event = _typeBuilder.DefineEvent(
                eventInfo.Name,
                eventInfo.Attributes,
                eventInfo.EventHandlerType);

            @event.SetAddOnMethod(addMethod);
            @event.SetRemoveOnMethod(removeMethod);
        }

        public void AddProperty(PropertyInfo propertyInfo)
        {
            Require.NotNull(propertyInfo, nameof(propertyInfo));

            var property = _typeBuilder.DefineProperty(
                propertyInfo.Name,
                propertyInfo.Attributes,
                propertyInfo.PropertyType,
                Type.EmptyTypes);

            if (propertyInfo.CanRead)
            {
                var getter = DefineGetter(propertyInfo);
                property.SetGetMethod(getter);
            }

            if (propertyInfo.CanWrite)
            {
                var setter = DefineSetter(propertyInfo);
                property.SetSetMethod(setter);
            }
        }

        private MethodBuilder DefineAddMethod(EventInfo eventInfo)
        {
            Debug.Assert(eventInfo != null);

            var addBuilder = _typeBuilder.DefineMethod(
                "add" + "_" + eventInfo.Name,
                s_MethodAttributes,
                typeof(void),
                new[] { eventInfo.EventHandlerType });

            var il = addBuilder.GetILGenerator();

            EmitILForEachView(
                il,
                () =>
                {
                    // Call the original add method
                    var originalAddMethod = eventInfo.GetAddMethod();
                    il.EmitCall(OpCodes.Callvirt, originalAddMethod, null);
                });

            // Return control
            il.Emit(OpCodes.Ret);

            return addBuilder;
        }

        private MethodBuilder DefineRemoveMethod(EventInfo eventInfo)
        {
            Debug.Assert(eventInfo != null);

            var removeBuilder = _typeBuilder.DefineMethod(
                "remove" + "_" + eventInfo.Name,
                s_MethodAttributes,
                typeof(void),
                new[] { eventInfo.EventHandlerType });

            var il = removeBuilder.GetILGenerator();

            EmitILForEachView(
                il,
                () =>
                {
                    // Call the original remove method
                    var originalRemoveMethod = eventInfo.GetRemoveMethod();
                    il.EmitCall(OpCodes.Callvirt, originalRemoveMethod, null);
                });

            // Return control
            il.Emit(OpCodes.Ret);

            return removeBuilder;
        }

        private MethodBuilder DefineGetter(PropertyInfo propertyInfo)
        {
            Debug.Assert(propertyInfo != null);

            var getBuilder = _typeBuilder.DefineMethod(
                "get" + "_" + propertyInfo.Name,
                s_MethodAttributes,
                propertyInfo.PropertyType,
                Type.EmptyTypes);

            var il = getBuilder.GetILGenerator();

            // Declare a local to store the return value in
            var local = il.DeclareLocal(propertyInfo.PropertyType);

            // Load the view instance on to the evaluation stack
            il.Emit(OpCodes.Ldarg, local.LocalIndex);

            // Call CompositeView<IViewType>.get_Views
            var getViews = GetCompositeViewViewsPropertyGetter();
            il.EmitCall(OpCodes.Call, getViews, null);

            // Call IEnumerable.First<IViewType>
            var firstView = typeof(Enumerable)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(mi => mi.Name == "First")
                .SingleOrDefault(mi =>
                {
                    var parameters = mi.GetParameters();
                    return parameters.Length == 1 &&
                        parameters[0].ParameterType.GUID == typeof(IEnumerable<>).GUID;
                })
                ?.MakeGenericMethod(new Type[] { _viewType });
            il.EmitCall(OpCodes.Call, firstView, null);

            // Call the original getter, eg. IViewType.get_SomeProperty
            var originalGetter = propertyInfo.GetGetMethod();
            il.EmitCall(OpCodes.Callvirt, originalGetter, null);

            // Push it from the evaluation stack to the local variable
            il.Emit(OpCodes.Stloc, local.LocalIndex);

            // Push it from the local variable back onto the evaluation stack
            il.Emit(OpCodes.Ldloc, local.LocalIndex);

            // Return control
            il.Emit(OpCodes.Ret);

            return getBuilder;
        }

        private MethodBuilder DefineSetter(PropertyInfo propertyInfo)
        {
            Debug.Assert(propertyInfo != null);

            var setBuilder = _typeBuilder.DefineMethod(
                "set" + "_" + propertyInfo.Name,
                s_MethodAttributes,
                typeof(void),
                new[] { propertyInfo.PropertyType });

            var il = setBuilder.GetILGenerator();

            EmitILForEachView(
                il,
                () =>
                {
                    // Call the original setter
                    var originalSetter = propertyInfo.GetSetMethod();
                    il.EmitCall(OpCodes.Callvirt, originalSetter, null);
                });

            // Return control
            il.Emit(OpCodes.Ret);

            return setBuilder;
        }

        private void EmitILForEachView(ILGenerator il, Action forEachAction)
        {
            Debug.Assert(il != null);
            Debug.Assert(forEachAction != null);

            // Declare the locals we need
            var viewLocal = il.DeclareLocal(_viewType);

            var enumeratorLocal = il.DeclareLocal(EnumerableType);

            var enumeratorContinueLocal = il.DeclareLocal(typeof(bool));

            // Load the view instance on to the evaluation stack
            il.Emit(OpCodes.Ldarg, viewLocal.LocalIndex);

            // Call CompositeView<IViewType>.get_Views
            var getViews = GetCompositeViewViewsPropertyGetter();
            il.EmitCall(OpCodes.Call, getViews, null);

            // Call IEnumerable<>.GetEnumerator
            var getViewsEnumerator = EnumerableType
                .GetMethod(
                    "GetEnumerator",
                    BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
            il.EmitCall(OpCodes.Callvirt, getViewsEnumerator, null);

            // Push the enumerator from the evaluation stack to the local variable
            il.Emit(OpCodes.Stloc, enumeratorLocal.LocalIndex);

            // Start a new exception block so that we can reliably dispose
            // the enumerator
            il.BeginExceptionBlock();

            // Define some of the labels we need
            var moveNextLabel = il.DefineLabel();
            var continueLabel = il.DefineLabel();
            var endFinallyLabel = il.DefineLabel();
            var exitLabel = il.DefineLabel();

            // Skip straight ahead to moveNextLabel
            il.Emit(OpCodes.Br_S, moveNextLabel);

            // Mark this point with with continueLabel
            il.MarkLabel(continueLabel);

            // Push the enumerator on to the evaluation stack
            il.Emit(OpCodes.Ldloc, enumeratorLocal);

            // Call IEnumerator<>.get_Current on the enumerator
            var getCurrent = EnumeratorType.GetProperty("Current")?.GetGetMethod();
            il.EmitCall(OpCodes.Callvirt, getCurrent, null);

            // Store the output from IEnumerator<>.get_Current into a local
            il.Emit(OpCodes.Stloc, viewLocal);

            // Push the view local back onto the evaluation stack
            il.Emit(OpCodes.Ldloc, viewLocal);

            // Push the incoming set value onto the evaluation stack
            il.Emit(OpCodes.Ldarg, 1);

            // Let the calling method inject some IL here
            forEachAction();

            // Mark this point with the moveNextLabel
            il.MarkLabel(moveNextLabel);

            // Push the enumerator local back onto the evaluation stack
            il.Emit(OpCodes.Ldloc, enumeratorLocal);

            // Call IEnumerator.MoveNext on the enumerator
            var moveNext = typeof(IEnumerator).GetMethod("MoveNext");
            il.EmitCall(OpCodes.Callvirt, moveNext, null);

            // Push the result of MoveNext from the evaluation stack to the local variable
            il.Emit(OpCodes.Stloc, enumeratorContinueLocal.LocalIndex);

            // Pull the result of MoveNext from the evaluation stack back onto the evaluation stack
            il.Emit(OpCodes.Ldloc, enumeratorContinueLocal.LocalIndex);

            // If MoveNext returned true, jump back to the continue label
            il.Emit(OpCodes.Brtrue_S, continueLabel);

            // Jump out of the try block
            il.Emit(OpCodes.Leave_S, exitLabel);

            // Start the finally block
            il.BeginFinallyBlock();

            // Push the enumerator onto the evaluation stack, then compare against null
            il.Emit(OpCodes.Ldloc, enumeratorLocal);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);

            // Pop the comparison result into our local
            il.Emit(OpCodes.Stloc, enumeratorContinueLocal);

            // If the comparison result was true, jump to the end of the finally block
            il.Emit(OpCodes.Ldloc, enumeratorContinueLocal);
            il.Emit(OpCodes.Brtrue_S, endFinallyLabel);

            // Push the enumerator onto the evaluation stack
            il.Emit(OpCodes.Ldloc, enumeratorLocal);

            // Call IDisposable.Dispose
            var dispose = typeof(IDisposable).GetMethod("Dispose");
            il.Emit(OpCodes.Callvirt, dispose);

            // Mark this point as exit point for our finally block
            il.MarkLabel(endFinallyLabel);

            // Close the try block
            il.EndExceptionBlock();

            // Mark this point as our exit point (used to get out of the try block)
            il.MarkLabel(exitLabel);
        }

        private MethodInfo GetCompositeViewViewsPropertyGetter()
        {
            // TODO: Throw on null value?
            // NB: Our definition of CompositeView<IViewType> ensures that the result is not null,
            // but we never know, this explains all the ?. below.
            return CompositeViewType
                .GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.NonPublic)
                .FirstOrDefault(pi => pi.Name == "Views" && pi.PropertyType == EnumerableType)
                ?.GetGetMethod(true);
        }
    }
}
