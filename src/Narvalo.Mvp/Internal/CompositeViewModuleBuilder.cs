// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    internal sealed class CompositeViewModuleBuilder
    {
        readonly ModuleBuilder _moduleBuilder;

        public CompositeViewModuleBuilder(ModuleBuilder moduleBuilder)
        {
            DebugCheck.NotNull(moduleBuilder);

            _moduleBuilder = moduleBuilder;
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
    }
}