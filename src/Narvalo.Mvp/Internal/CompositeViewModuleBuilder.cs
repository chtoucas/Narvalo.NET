// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    internal sealed class CompositeViewModuleBuilder
    {
        const string AssemblyName_ = "Narvalo.Mvp.CompositeViews";

        readonly string _assemblyName;
        readonly ModuleBuilder _moduleBuilder;

        public CompositeViewModuleBuilder(string assemblyName)
        {
            DebugCheck.NotNullOrEmpty(assemblyName);

            _assemblyName = assemblyName;
            _moduleBuilder = CreateModuleBuilder_();
        }

        public TypeBuilder DefineType(Type viewType)
        {
            DebugCheck.NotNull(viewType);

            // Create a generic type of type "CompositeView<ITestView>".
            var parentType = typeof(CompositeView<>).MakeGenericType(viewType);

            var interfaces = new[] { viewType };

            return _moduleBuilder.DefineType(
                viewType.FullName + "__@CompositeView",
                TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class,
                parentType,
                interfaces);
        }

        ModuleBuilder CreateModuleBuilder_()
        {
            var assemblyName = new AssemblyName(_assemblyName);
            // FIXME: Why does it fail when we add the "SecurityTransparent" attribute?
            //var attributeBuilders = new CustomAttributeBuilder[] {
            //    new CustomAttributeBuilder(
            //        typeof(SecurityTransparentAttribute).GetConstructor(Type.EmptyTypes), 
            //        new Object[0])
            //};

            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly
            (
                assemblyName,
                AssemblyBuilderAccess.Run
                //, attributeBuilders
            );

            return assembly.DefineDynamicModule(assemblyName.Name);
        }
    }
}