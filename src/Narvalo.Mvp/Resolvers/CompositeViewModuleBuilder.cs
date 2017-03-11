// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    public sealed partial class CompositeViewModuleBuilder
    {
        private const string ASSEMBLY_NAME = "Narvalo.Mvp.CompositeViews";

        private readonly string _assemblyName;
        private readonly Lazy<ModuleBuilder> _moduleBuilder;

        public CompositeViewModuleBuilder(string assemblyName)
        {
            Require.NotNullOrEmpty(assemblyName, nameof(assemblyName));

            _assemblyName = assemblyName;
            _moduleBuilder = new Lazy<ModuleBuilder>(CreateModuleBuilder);
        }

        public TypeBuilder DefineType(Type viewType)
        {
            Require.NotNull(viewType, nameof(viewType));

            // Create a generic type of type "CompositeView<ITestView>".
            var type = typeof(CompositeView<>);

            var parentType = type.MakeGenericType(new Type[] { viewType });

            var interfaces = new[] { viewType };

            var typeBuilder = _moduleBuilder.Value.DefineType(
                viewType.FullName + "__@CompositeView",
                TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class,
                parentType,
                interfaces);

            return typeBuilder;
        }

        private ModuleBuilder CreateModuleBuilder()
        {
            var assemblyName = new AssemblyName(_assemblyName);

            // FIXME: Why does it fail when we add the "SecurityTransparent" attribute?
            // Maybe because we didn't add Narvalo.Mvp to the $(_ForceTransparency) property
            // in Make.CustomAfter.targets.
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
