// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Diagnostics;
    using System.Reflection.Emit;

    using Narvalo.Mvp.Properties;

    public sealed class /*Default*/PresenterConstructorResolver : IPresenterConstructorResolver
    {
        public DynamicMethod Resolve(Type presenterType, Type viewType)
        {
            Require.NotNull(presenterType, nameof(presenterType));
            Require.NotNull(viewType, nameof(viewType));

            Debug.Assert(
                typeof(IPresenter<IView>).IsAssignableFrom(presenterType),
                "Asserts 'presenterType' is of type 'IPresenter<IView>'.");
            Debug.Assert(typeof(IView).IsAssignableFrom(viewType), "Asserts 'viewType' is of type 'IView'.");

            Trace.TraceInformation(
                "[PresenterConstructorResolver] Attempting to resolve '{0}'.", presenterType.FullName);

            if (presenterType.IsNotPublic)
            {
                throw new ArgumentException(Format.Current(
                        Strings.PresenterConstructorResolver_PresenterIsNotPublic,
                        presenterType.FullName),
                    nameof(presenterType));
            }

            var ctor = presenterType.GetConstructor(new[] { viewType });
            if (ctor == null)
            {
                throw new ArgumentException(Format.Current(
                        Strings.PresenterConstructorResolver_MissingPublicCtor,
                        presenterType.FullName,
                        viewType.FullName),
                    nameof(presenterType));
            }

            // Using DynamicMethod and ILGenerator allows us to hold on to a
            // JIT-ed constructor call, which gives us an insanely fast way
            // to create type instances on the fly. This provides a surprising
            // performance improvement over basic reflection in applications
            // that create lots of presenters, which is common.
            var dynamicMethod = new DynamicMethod(
                "DynamicConstructor",
                presenterType,
                new[] { viewType },
                presenterType.Module,
                skipVisibility: false);

            var il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);

            return dynamicMethod;
        }
    }
}
