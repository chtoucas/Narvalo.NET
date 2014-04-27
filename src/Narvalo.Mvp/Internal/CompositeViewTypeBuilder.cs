// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    internal sealed class CompositeViewTypeBuilder
    {
        const string AssemblyName_ = "Narvalo.Mvp.CompositeViewTypes";

        static readonly MethodAttributes MethodAttributes_
            = MethodAttributes.Public
            | MethodAttributes.SpecialName
            | MethodAttributes.HideBySig
            | MethodAttributes.Virtual;

        readonly TypeBuilder _typeBuilder;
        readonly Type _viewType;

        CompositeViewTypeBuilder(Type viewType, TypeBuilder typeBuilder)
        {
            _viewType = viewType;
            _typeBuilder = typeBuilder;
        }

        public static CompositeViewTypeBuilder Create(Type viewType)
        {
            var module = BuildModule_(AppDomain.CurrentDomain);

            var typeBuilder = BuildTypeBuilder_(module, viewType);

            return new CompositeViewTypeBuilder(viewType, typeBuilder);
        }

        public Type Build()
        {
            return _typeBuilder.CreateType();
        }

        public void AddEvent(EventInfo eventInfo)
        {
            if (eventInfo.EventHandlerType == null) {
                throw new ArgumentException(string.Format(
                    CultureInfo.InvariantCulture,
                    "The supplied event {0} from {1} does not have the event handler type specified.",
                    eventInfo.Name,
                    eventInfo.ReflectedType.Name),
                    "eventInfo");
            }

            var addMethod = DefineAddMethod_(eventInfo);
            var removeMethod = DefineRemoveMethod_(eventInfo);

            var @event = _typeBuilder.DefineEvent(
                eventInfo.Name,
                eventInfo.Attributes,
                eventInfo.EventHandlerType);

            @event.SetAddOnMethod(addMethod);
            @event.SetRemoveOnMethod(removeMethod);
        }

        public void AddProperty(PropertyInfo propertyInfo)
        {
            var property = _typeBuilder.DefineProperty(
                propertyInfo.Name,
                propertyInfo.Attributes,
                propertyInfo.PropertyType,
                Type.EmptyTypes);

            if (propertyInfo.CanRead) {
                var getter = DefineGetter_(propertyInfo);
                property.SetGetMethod(getter);
            }

            if (propertyInfo.CanWrite) {
                var setter = DefineSetter_(propertyInfo);
                property.SetSetMethod(setter);
            }
        }

        static ModuleBuilder BuildModule_(AppDomain appDomain)
        {
            var assemblyName = new AssemblyName(AssemblyName_);
            var attributeBuilders = new CustomAttributeBuilder[]
            {
                // FIXME: Why does it fail when we add the "SecurityTransparent" attribute?
                //    new CustomAttributeBuilder(
                //        typeof(SecurityTransparentAttribute).GetConstructor(Type.EmptyTypes), 
                //        new Object[0])
            };

            var assembly = appDomain.DefineDynamicAssembly
            (
                assemblyName,
                AssemblyBuilderAccess.Run,
                attributeBuilders
            );

            return assembly.DefineDynamicModule(assemblyName.Name);
        }

        static TypeBuilder BuildTypeBuilder_(ModuleBuilder module, Type viewType)
        {
            // Create a generic type of type "CompositeView<ITestView>".
            var parentType = typeof(CompositeView<>).MakeGenericType(viewType);

            var interfaces = new[] { viewType };

            return module.DefineType(
                viewType.FullName + "__@CompositeView",
                TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class,
                parentType,
                interfaces);
        }

        MethodBuilder DefineAddMethod_(EventInfo eventInfo)
        {
            var addBuilder = _typeBuilder.DefineMethod(
                "add" + "_" + eventInfo.Name,
                MethodAttributes_,
                typeof(void),
                new[] { eventInfo.EventHandlerType });

            var il = addBuilder.GetILGenerator();

            EmitILForEachView_(il, () =>
            {
                // Call the original add method
                var originalAddMethod = eventInfo.GetAddMethod();
                il.EmitCall(OpCodes.Callvirt, originalAddMethod, null);
            });

            // Return control
            il.Emit(OpCodes.Ret);

            return addBuilder;
        }

        MethodBuilder DefineRemoveMethod_(EventInfo eventInfo)
        {
            var removeBuilder = _typeBuilder.DefineMethod(
                "remove" + "_" + eventInfo.Name,
                MethodAttributes_,
                typeof(void),
                new[] { eventInfo.EventHandlerType });

            var il = removeBuilder.GetILGenerator();

            EmitILForEachView_(il,
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

        MethodBuilder DefineGetter_(PropertyInfo propertyInfo)
        {
            /*
             * Produces something functionally equivalent to this:
             * 

get
{
    return Views.First().Model;
}

             * 
             * It does this by emitting IL like this:
             * 

.method public hidebysig newslot specialname virtual final 
    instance class WebFormsMvp.FeatureDemos.Logic.Views.Models.CompositeDemoViewModel 
    get_Model() cil managed
{
    // Code size       22 (0x16)
    .maxstack  1
    .locals init ([0] class WebFormsMvp.FeatureDemos.Logic.Views.Models.CompositeDemoViewModel CS$1$0000)
    IL_0000:  nop
    IL_0001:  ldarg.0
    IL_0002:  call       instance class [mscorlib]System.Collections.Generic.IEnumerable`1<!0> class [WebFormsMvp]WebFormsMvp.CompositeView`1<class WebFormsMvp.FeatureDemos.Logic.Views.ICompositeDemoView>::get_Views()
    IL_0007:  call       !!0 [System.Core]System.Linq.Enumerable::First<class WebFormsMvp.FeatureDemos.Logic.Views.ICompositeDemoView>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>)
    IL_000c:  callvirt   instance !0 class [WebFormsMvp]WebFormsMvp.IView`1<class WebFormsMvp.FeatureDemos.Logic.Views.Models.CompositeDemoViewModel>::get_Model()
    IL_0011:  stloc.0
    IL_0012:  br.s       IL_0014
    IL_0014:  ldloc.0
    IL_0015:  ret
} // end of method CompositeDemoViewComposite::get_Model

             * 
             */

            var getBuilder = _typeBuilder.DefineMethod(
                "get" + "_" + propertyInfo.Name,
                MethodAttributes_,
                propertyInfo.PropertyType,
                Type.EmptyTypes);

            var il = getBuilder.GetILGenerator();

            // Declare a local to store the return value in
            var local = il.DeclareLocal(propertyInfo.PropertyType);

            // Load the view instance on to the evaluation stack
            il.Emit(OpCodes.Ldarg, local.LocalIndex);

            // Call CompositeView<IViewType>.get_Views
            var getViews = typeof(CompositeView<>)
                .MakeGenericType(_viewType)
                .GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.NonPublic)
                .First(pi => pi.Name == "Views"
                    && pi.PropertyType == typeof(IEnumerable<>).MakeGenericType(_viewType))
                .GetGetMethod(true);
            il.EmitCall(OpCodes.Call, getViews, null);

            // Call IEnumerable.First<IViewType>
            var firstView = typeof(Enumerable)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(mi => mi.Name == "First")
                .Single(mi =>
                {
                    var parameters = mi.GetParameters();
                    return parameters.Length == 1 &&
                        parameters[0].ParameterType.GUID == typeof(IEnumerable<>).GUID;
                })
                .MakeGenericMethod(_viewType);
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

        MethodBuilder DefineSetter_(PropertyInfo propertyInfo)
        {
            /*
             * Produces something functionally equivalent to this:
             * 

set
{
    foreach(var view in Views)
        view.Model = value;
}

             * 
             * It does this by emitting IL like this:
             * 

.method public hidebysig newslot specialname virtual final 
    instance void  set_Model(class WebFormsMvp.FeatureDemos.Logic.Views.Models.CompositeDemoViewModel 'value') cil managed
{
    // Code size       61 (0x3d)
    .maxstack  2
    .locals init ([0] class WebFormsMvp.FeatureDemos.Logic.Views.ICompositeDemoView view,
        [1] class [mscorlib]System.Collections.Generic.IEnumerator`1<class WebFormsMvp.FeatureDemos.Logic.Views.ICompositeDemoView> CS$5$0000,
        [2] bool CS$4$0001)
    IL_0000:  nop
    IL_0001:  nop
    IL_0002:  ldarg.0
    IL_0003:  call       instance class [mscorlib]System.Collections.Generic.IEnumerable`1<!0> class [WebFormsMvp]WebFormsMvp.CompositeView`1<class WebFormsMvp.FeatureDemos.Logic.Views.ICompositeDemoView>::get_Views()
    IL_0008:  callvirt   instance class [mscorlib]System.Collections.Generic.IEnumerator`1<!0> class [mscorlib]System.Collections.Generic.IEnumerable`1<class WebFormsMvp.FeatureDemos.Logic.Views.ICompositeDemoView>::GetEnumerator()
    IL_000d:  stloc.1
    .try
    {
        IL_000e:  br.s       IL_001f

        IL_0010:  ldloc.1
        IL_0011:  callvirt   instance !0 class [mscorlib]System.Collections.Generic.IEnumerator`1<class WebFormsMvp.FeatureDemos.Logic.Views.ICompositeDemoView>::get_Current()
        IL_0016:  stloc.0
        IL_0017:  ldloc.0
        IL_0018:  ldarg.1
        IL_0019:  callvirt   instance void class [WebFormsMvp]WebFormsMvp.IView`1<class WebFormsMvp.FeatureDemos.Logic.Views.Models.CompositeDemoViewModel>::set_Model(!0)
        IL_001e:  nop
        IL_001f:  ldloc.1
        IL_0020:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
        IL_0025:  stloc.2
        IL_0026:  ldloc.2
        IL_0027:  brtrue.s   IL_0010

        IL_0029:  leave.s    IL_003b

    }  // end .try
    finally
    {
        IL_002b:  ldloc.1
        IL_002c:  ldnull
        IL_002d:  ceq
        IL_002f:  stloc.2
        IL_0030:  ldloc.2
        IL_0031:  brtrue.s   IL_003a

        IL_0033:  ldloc.1
        IL_0034:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
        IL_0039:  nop
        IL_003a:  endfinally
    }  // end handler
    IL_003b:  nop
    IL_003c:  ret
} // end of method CompositeDemoViewComposite::set_Model

             */

            var setBuilder = _typeBuilder.DefineMethod(
                "set" + "_" + propertyInfo.Name,
                MethodAttributes_,
                typeof(void),
                new[] { propertyInfo.PropertyType });

            var il = setBuilder.GetILGenerator();

            EmitILForEachView_(il, () =>
            {
                // Call the original setter
                var originalSetter = propertyInfo.GetSetMethod();
                il.EmitCall(OpCodes.Callvirt, originalSetter, null);
            });

            // Return control
            il.Emit(OpCodes.Ret);

            return setBuilder;
        }

        void EmitILForEachView_(ILGenerator il, Action forEachAction)
        {
            // Declare the locals we need
            var viewLocal = il.DeclareLocal(_viewType);
            var enumeratorLocal = il.DeclareLocal(typeof(IEnumerable<>).MakeGenericType(_viewType));
            var enumeratorContinueLocal = il.DeclareLocal(typeof(bool));

            // Load the view instance on to the evaluation stack
            il.Emit(OpCodes.Ldarg, viewLocal.LocalIndex);

            // Call CompositeView<IViewType>.get_Views
            var getViews = typeof(CompositeView<>)
                .MakeGenericType(_viewType)
                .GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.NonPublic)
                .First(pi => pi.Name == "Views" &&
                    pi.PropertyType == typeof(IEnumerable<>).MakeGenericType(_viewType))
                .GetGetMethod(true);
            il.EmitCall(OpCodes.Call, getViews, null);

            // Call IEnumerable<>.GetEnumerator
            var getViewsEnumerator = typeof(IEnumerable<>)
                .MakeGenericType(_viewType)
                .GetMethod("GetEnumerator",
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
            var getCurrent = typeof(IEnumerator<>)
                .MakeGenericType(_viewType)
                .GetProperty("Current")
                .GetGetMethod();
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
            var dispose =
                typeof(IDisposable)
                .GetMethod("Dispose");
            il.Emit(OpCodes.Callvirt, dispose);

            // Mark this point as exit point for our finally block
            il.MarkLabel(endFinallyLabel);

            // Close the try block
            il.EndExceptionBlock();

            // Mark this point as our exit point (used to get out of the try block)
            il.MarkLabel(exitLabel);
        }
    }
}