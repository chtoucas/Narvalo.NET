// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    internal static class CompositeViewAssembly
    {
        const string AssemblyName_ = "Narvalo.Mvp.CompositeViews";

        static readonly Lazy<ModuleBuilder> ModuleBuilder_
            = new Lazy<ModuleBuilder>(CreateModule_);

        public static TypeBuilder DefineType(Type viewType)
        {
            // Create a generic type of type "CompositeView<ITestView>".
            var parentType = typeof(CompositeView<>).MakeGenericType(viewType);

            var interfaces = new[] { viewType };

            return ModuleBuilder_.Value.DefineType(
                viewType.FullName + "__@CompositeView",
                TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class,
                parentType,
                interfaces);
        }

        static ModuleBuilder CreateModule_()
        {
            var assemblyName = new AssemblyName(AssemblyName_);
            var attributeBuilders = new CustomAttributeBuilder[]
            {
                // FIXME: Why does it fail when we add the "SecurityTransparent" attribute?
                //    new CustomAttributeBuilder(
                //        typeof(SecurityTransparentAttribute).GetConstructor(Type.EmptyTypes), 
                //        new Object[0])
            };

            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly
            (
                assemblyName,
                AssemblyBuilderAccess.Run,
                attributeBuilders
            );

            return assembly.DefineDynamicModule(assemblyName.Name);
        }
    }
}