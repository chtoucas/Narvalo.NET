// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Diagnostics;
#if CONTRACTS_FULL // Contract Class and Object Invariants.
    using System.Diagnostics.Contracts;
#endif
    using System.Reflection;

    using Narvalo;
    using Narvalo.Mvp.Properties;
    using Narvalo.Mvp.Resolvers;

    using static System.Diagnostics.Contracts.Contract;

    /// <remarks>
    /// WARNING: This class can not be used for presenters that do not have a constructor
    /// accepting the view as a single parameter.
    /// </remarks>
    public sealed class /*Default*/PresenterFactory : IPresenterFactory
    {
        private readonly IPresenterConstructorResolver _constructorResolver;

        public PresenterFactory()
            : this(new PresenterConstructorResolver()) { }

        public PresenterFactory(IPresenterConstructorResolver constructorResolver)
            : this(constructorResolver, true)
        {
            Expect.NotNull(constructorResolver);
        }

        public PresenterFactory(
            IPresenterConstructorResolver constructorResolver,
            bool enableCache)
        {
            Require.NotNull(constructorResolver, nameof(constructorResolver));

            _constructorResolver = enableCache
                 ? new CachedPresenterConstructorResolver(constructorResolver)
                 : constructorResolver;
        }

        public IPresenter Create(Type presenterType, Type viewType, IView view)
        {
            Require.NotNull(presenterType, nameof(presenterType));
            Require.NotNull(viewType, nameof(viewType));
            Require.NotNull(view, nameof(view));

            var ctor = _constructorResolver.Resolve(presenterType, viewType);

            Trace.TraceInformation(
                    "[PresenterFactory] Constructing presenter '{0}' for view '{1}', view '{2}'.",
                    presenterType.FullName,
                    viewType.FullName,
                    view.GetType().FullName);

            try
            {
                // NB: The return value might be null.
                return (IPresenter)ctor.Invoke(null, new[] { view });
            }
            catch (Exception ex)
            {
                var originalException = ex;

                if (ex is TargetInvocationException && ex.InnerException != null)
                {
                    originalException = ex.InnerException;
                }

                throw new PresenterBindingException(Format.Current(
                        Strings.PresenterFactory_PresenterCtorFailed,
                        presenterType.FullName),
                    originalException);
            }
        }

        public void Release(IPresenter presenter)
        {
            var disposablePresenter = presenter as IDisposable;

            if (disposablePresenter != null)
            {
                disposablePresenter.Dispose();
            }
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_constructorResolver != null);
        }

#endif
    }
}