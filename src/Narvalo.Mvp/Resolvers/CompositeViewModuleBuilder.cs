// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    public sealed class CompositeViewModuleBuilder
    {
        private const string ASSEMBLY_NAME = "Narvalo.Mvp.CompositeViews";

        private readonly string _assemblyName;
        private readonly Lazy<ModuleBuilder> _moduleBuilder;

        public CompositeViewModuleBuilder(string assemblyName)
        {
            Require.NotNullOrEmpty(assemblyName, "assemblyName");

            _assemblyName = assemblyName;
            _moduleBuilder = new Lazy<ModuleBuilder>(CreateModuleBuilder_);
        }

        public TypeBuilder DefineType(Type viewType)
        {
            Require.NotNull(viewType, "viewType");

            // Create a generic type of type "CompositeView<ITestView>".
            var parentType = typeof(CompositeView<>).MakeGenericType(viewType);

            var interfaces = new[] { viewType };

            return _moduleBuilder.Value.DefineType(
                viewType.FullName + "__@CompositeView",
                TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class,
                parentType,
                interfaces);
        }

        private ModuleBuilder CreateModuleBuilder_()
        {
            var assemblyName = new AssemblyName(_assemblyName);

            // FIXME: Why does it fail when we add the "SecurityTransparent" attribute?
            // var attributeBuilders = new CustomAttributeBuilder[] {
            //    new CustomAttributeBuilder(
            //        typeof(SecurityTransparentAttribute).GetConstructor(Type.EmptyTypes), 
            //        new Object[0])
            // };
            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(
                assemblyName,
                AssemblyBuilderAccess.Run);

            return assembly.DefineDynamicModule(assemblyName.Name);
        }
    }
}