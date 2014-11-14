// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Narvalo;
    using Narvalo.Mvp.Resolvers;

    /// <remarks>
    /// WARNING: This class can not be used for presenters that do not have a constructor
    /// accepting the view as a single parameter.
    /// </remarks>
    public sealed class /*Default*/PresenterFactory : IPresenterFactory
    {
        readonly IPresenterConstructorResolver _constructorResolver;

        public PresenterFactory()
            : this(new PresenterConstructorResolver()) { }

        public PresenterFactory(IPresenterConstructorResolver constructorResolver)
            : this(constructorResolver, true) { }

        public PresenterFactory(
            IPresenterConstructorResolver constructorResolver,
            bool enableCache)
        {
            Require.NotNull(constructorResolver, "constructorResolver");

            _constructorResolver = enableCache
                 ? new CachedPresenterConstructorResolver(constructorResolver)
                 : constructorResolver;
        }

        public IPresenter Create(Type presenterType, Type viewType, IView view)
        {
            Require.NotNull(presenterType, "presenterType");
            Require.NotNull(viewType, "viewType");
            Require.NotNull(view, "view");

            var ctor = _constructorResolver.Resolve(presenterType, viewType);

            Tracer.Info(
                this,
                @"Constructing presenter ""{0}"" for view ""{1}"", view ""{2}"".",
                presenterType.FullName,
                viewType.FullName,
                view.GetType().FullName);

            try {
                return (IPresenter)ctor.Invoke(null, new[] { view });
            }
            catch (Exception ex) {
                var originalException = ex;

                if (ex is TargetInvocationException && ex.InnerException != null) {
                    originalException = ex.InnerException;
                }

                throw new PresenterBindingException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "An exception was thrown whilst trying to create an instance of {0}. Check the InnerException for more information.",
                        presenterType.FullName),
                    originalException);
            }
        }

        public void Release(IPresenter presenter)
        {
            var disposablePresenter = presenter as IDisposable;

            if (disposablePresenter != null) {
                disposablePresenter.Dispose();
            }
        }
    }
}