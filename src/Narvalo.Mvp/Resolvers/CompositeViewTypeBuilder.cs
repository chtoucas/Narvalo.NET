// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
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
                Warrant.NotNull<Type>();

                if (_compositeViewType == null)
                {
                    var type = typeof(CompositeView<>);
                    Contract.Assume(type.GetGenericArguments()?.Length == 1, "Obvious per definition of CompositeView<>.");
                    Contract.Assume(type.IsGenericTypeDefinition, "Obvious per definition of CompositeView<>.");

                    _compositeViewType = type.MakeGenericType(new Type[] { _viewType });
                }

                return _compositeViewType;
            }
        }

        private Type EnumerableType
        {
            get
            {
                Warrant.NotNull<Type>();

                if (_enumerableType == null)
                {
                    var type = typeof(IEnumerable<>);
                    Contract.Assume(type.GetGenericArguments()?.Length == 1, "Obvious per definition of IEnumerable<>.");
                    Contract.Assume(type.IsGenericTypeDefinition, "Obvious per definition of IEnumerable<>.");

                    _enumerableType = type.MakeGenericType(new Type[] { _viewType });
                }

                return _enumerableType;
            }
        }

        private Type EnumeratorType
        {
            get
            {
                Warrant.NotNull<Type>();

                if (_enumeratorType == null)
                {
                    var type = typeof(IEnumerator<>);
                    Contract.Assume(type.GetGenericArguments()?.Length == 1, "Obvious per definition of IEnumerator<>.");
                    Contract.Assume(type.IsGenericTypeDefinition, "Obvious per definition of IEnumerator<>.");

                    _enumeratorType = type.MakeGenericType(new Type[] { _viewType });
                }

                return _enumeratorType;
            }
        }

        public Type Build()
        {
            Warrant.NotNull<Type>();

            var type = _typeBuilder.CreateType();
            Contract.Assume(type != null, "Extern: BCL.");

            return type;
        }

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
            Contract.Assume(@event != null, "Extern: BCL.");

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
            Contract.Assume(property != null, "Extern: BCL.");

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
            Demand.NotNull(eventInfo);

            var addBuilder = _typeBuilder.DefineMethod(
                "add" + "_" + eventInfo.Name,
                s_MethodAttributes,
                typeof(void),
                new[] { eventInfo.EventHandlerType });
            Contract.Assume(addBuilder != null, "Extern: BCL.");

            var il = addBuilder.GetILGenerator();
            Contract.Assume(il != null, "Extern: BCL.");

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
            Demand.NotNull(eventInfo);

            var removeBuilder = _typeBuilder.DefineMethod(
                "remove" + "_" + eventInfo.Name,
                s_MethodAttributes,
                typeof(void),
                new[] { eventInfo.EventHandlerType });
            Contract.Assume(removeBuilder != null, "Extern: BCL.");

            var il = removeBuilder.GetILGenerator();
            Contract.Assume(il != null, "Extern: BCL.");

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
            Demand.NotNull(propertyInfo);

            var getBuilder = _typeBuilder.DefineMethod(
                "get" + "_" + propertyInfo.Name,
                s_MethodAttributes,
                propertyInfo.PropertyType,
                Type.EmptyTypes);
            Contract.Assume(getBuilder != null, "Extern: BCL.");

            var il = getBuilder.GetILGenerator();
            Contract.Assume(il != null, "Extern: BCL.");

            // Declare a local to store the return value in
            var local = il.DeclareLocal(propertyInfo.PropertyType);
            Contract.Assume(local != null, "Extern: BCL.");

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
            Demand.NotNull(propertyInfo);

            var setBuilder = _typeBuilder.DefineMethod(
                "set" + "_" + propertyInfo.Name,
                s_MethodAttributes,
                typeof(void),
                new[] { propertyInfo.PropertyType });
            Contract.Assume(setBuilder != null, "Extern: BCL.");

            var il = setBuilder.GetILGenerator();
            Contract.Assume(il != null, "Extern: BCL.");

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
            Demand.NotNull(il);
            Demand.NotNull(forEachAction);

            // Declare the locals we need
            var viewLocal = il.DeclareLocal(_viewType);
            Contract.Assume(viewLocal != null, "Extern: BCL.");

            var enumeratorLocal = il.DeclareLocal(EnumerableType);
            Contract.Assume(enumeratorLocal != null, "Extern: BCL.");

            var enumeratorContinueLocal = il.DeclareLocal(typeof(bool));
            Contract.Assume(enumeratorContinueLocal != null, "Extern: BCL.");

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

#if CONTRACTS_FULL

namespace Narvalo.Mvp.Resolvers
{
    using System.Diagnostics.Contracts;

    public sealed partial class CompositeViewTypeBuilder
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_viewType != null);
            Contract.Invariant(_typeBuilder != null);
        }
    }
}

#endif
