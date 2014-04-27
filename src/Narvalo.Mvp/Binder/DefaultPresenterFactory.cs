// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Reflection;
    using System.Reflection.Emit;
    using Narvalo;

    // WARNING: This class can only be used for presenters with a constructor
    // accepting the view as a single parameter.
    public sealed class DefaultPresenterFactory : IPresenterFactory
    {
        // REVIEW: We use a concurrent dictionary as we expect to deal mostly with read operations
        // and to only do very few updates. Also note that, in most cases, the IPresenterFactory 
        // instance shall be unique during the entire lifetime of the application: PresenterBinder uses
        // the static property CompositeViewTypeBuilder.Current.Factory.
        static readonly ConcurrentDictionary<string, DynamicMethod> Cache_
            = new ConcurrentDictionary<string, DynamicMethod>();
       
        public IPresenter Create(Type presenterType, Type viewType, IView view)
        {
            Require.NotNull(presenterType, "presenterType");
            Require.NotNull(viewType, "viewType");
            Require.NotNull(view, "view");

            var ctor = GetConstructor_(presenterType, viewType);

            try {
                return (IPresenter)ctor.Invoke(null, new[] { view });
            }
            catch (Exception ex) {
                var originalException = ex;

                if (ex is TargetInvocationException && ex.InnerException != null) {
                    originalException = ex.InnerException;
                }

                throw new InvalidOperationException(String.Format(
                        CultureInfo.InvariantCulture,
                        "An exception was thrown whilst trying to create an instance of {0}. Check the InnerException for more information.",
                        presenterType.FullName),
                    originalException
                );
            }
        }

        public void Release(IPresenter presenter)
        {
            var disposablePresenter = presenter as IDisposable;

            if (disposablePresenter != null) {
                disposablePresenter.Dispose();
            }
        }

        static DynamicMethod GetConstructor_(Type presenterType, Type viewType)
        {
            // We need to scope the cache against both the presenter type and the view type.
            var cacheKey = String.Join("__:__", new[]
            {
                presenterType.AssemblyQualifiedName,
                viewType.AssemblyQualifiedName
            });

            return Cache_.GetOrAdd(cacheKey, _ => CreateConstructor_(presenterType, viewType));
        }

        static DynamicMethod CreateConstructor_(Type presenterType, Type viewType)
        {
            if (presenterType.IsNotPublic) {
                throw new ArgumentException(String.Format(
                    CultureInfo.InvariantCulture,
                    "{0} does not meet accessibility requirements. For the WebFormsMvp framework to be able to call it, it must be public. Make the type public, or set PresenterBinder.Factory to an implementation that can access this type.",
                    presenterType.FullName),
                    "presenterType");
            }

            var ctor = presenterType.GetConstructor(new[] { viewType });
            if (ctor == null) {
                throw new ArgumentException(String.Format(
                    CultureInfo.InvariantCulture,
                    "{0} is missing an expected constructor, or the constructor is not accessible. We tried to execute code equivalent to: new {0}({1} view). Add a public constructor with a compatible signature, or set PresenterBinder.Factory to an implementation that can supply constructor dependencies.",
                    presenterType.FullName,
                    viewType.FullName),
                    "presenterType");
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
                false /* skipVisibility */);

            var ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Newobj, ctor);
            ilGenerator.Emit(OpCodes.Ret);

            return dynamicMethod;
        }
    }
}