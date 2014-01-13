﻿namespace Narvalo.Presentation.Mvp.Simple
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Reflection.Emit;
    using Narvalo.Collections;

    /// <summary>
    /// Provides a default implementation of <see cref="IPresenterFactory"/>.
    /// </summary>
    public class DefaultPresenterFactory : IPresenterFactory
    {
        static readonly IDictionary<string, DynamicMethod> BuildMethodCache_
           = new Dictionary<string, DynamicMethod>();

        /// <summary>
        /// Creates a new instance of the specific presenter type, for the specified
        /// view type and instance.
        /// </summary>
        /// <param name="presenterType">The type of presenter to create.</param>
        /// <param name="viewType">The type of the view as defined by the binding that matched.</param>
        /// <param name="view">The view instance to bind this presenter to.</param>
        /// <returns>An instantitated presenter.</returns>
        public IPresenter Create(Type presenterType, Type viewType, IView view)
        {
            Requires.NotNull(presenterType, "presenterType");
            Requires.NotNull(viewType, "viewType");
            Requires.NotNull(view, "view");

            var buildMethod = GetBuildMethod(presenterType, viewType);

            try {
                return (IPresenter)buildMethod.Invoke(null, new[] { view });
            }
            catch (Exception ex) {
                var originalException = ex;

                if (ex is TargetInvocationException && ex.InnerException != null) {
                    originalException = ex.InnerException;
                }

                throw new InvalidOperationException
                (
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "An exception was thrown whilst trying to create an instance of {0}. Check the InnerException for more information.",
                        presenterType.FullName),
                    originalException
                );
            }
        }

        /// <summary>
        /// Releases the specified presenter from any of its lifestyle demands.
        /// </summary>
        /// <param name="presenter">The presenter to release.</param>
        public void Release(IPresenter presenter)
        {
            var disposablePresenter = presenter as IDisposable;
            if (disposablePresenter != null) {
                disposablePresenter.Dispose();
            }
        }

        internal static DynamicMethod GetBuildMethod(Type presenterType, Type viewType)
        {
            // We need to scope the cache against both the presenter type and the view type.
            var cacheKey = String.Join("__:__", new[] {
                presenterType.AssemblyQualifiedName,
                viewType.AssemblyQualifiedName
            });

            return BuildMethodCache_.GetOrCreateValue(cacheKey,
                () => GetBuildMethodInternal(presenterType, viewType));
        }

        internal static DynamicMethod GetBuildMethodInternal(Type presenterType, Type viewType)
        {
            if (presenterType.IsNotPublic) {
                throw new ArgumentException(String.Format(
                    CultureInfo.InvariantCulture,
                    "{0} does not meet accessibility requirements. For the WebFormsMvp framework to be able to call it, it must be public. Make the type public, or set PresenterBinder.Factory to an implementation that can access this type.",
                    presenterType.FullName),
                    "presenterType");
            }

            var constructor = presenterType.GetConstructor(new[] { viewType });
            if (constructor == null) {
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
            var dynamicMethod = new DynamicMethod("DynamicConstructor", presenterType, new[] { viewType }, presenterType.Module, false /* skipVisibility */);
            var ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Newobj, constructor);
            ilGenerator.Emit(OpCodes.Ret);

            return dynamicMethod;
        }
    }
}