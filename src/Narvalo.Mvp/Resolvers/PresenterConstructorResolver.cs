// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection.Emit;

    public sealed class /*Default*/PresenterConstructorResolver : IPresenterConstructorResolver
    {
        public DynamicMethod Resolve(Type presenterType, Type viewType)
        {
            Require.NotNull(presenterType, "presenterType");
            Require.NotNull(viewType, "viewType");

            Debug.Assert(typeof(IPresenter<IView>).IsAssignableFrom(presenterType));
            Debug.Assert(typeof(IView).IsAssignableFrom(viewType));

            Tracer.Info(this, @"Attempting to resolve ""{0}"".", presenterType.FullName);

            if (presenterType.IsNotPublic) {
                throw new ArgumentException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} does not meet accessibility requirements. For the framework to be able to call it, it must be public. Make the type public, or use a IPresenterFactory that can access this type.",
                        presenterType.FullName),
                    "input");
            }

            var ctor = presenterType.GetConstructor(new[] { viewType });
            if (ctor == null) {
                throw new ArgumentException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} is missing an expected constructor, or the constructor is not accessible. We tried to execute code equivalent to: new {0}({1} view). Add a public constructor with a compatible signature, or use a IPresenterFactory that can supply constructor dependencies.",
                        presenterType.FullName,
                        viewType.FullName),
                    "input");
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
