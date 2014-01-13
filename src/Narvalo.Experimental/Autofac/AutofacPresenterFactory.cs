using Autofac;
using Autofac.Core;

namespace Narvalo.Autofac
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Presentation.Mvp;

    public class AutofacPresenterFactory : IPresenterFactory
    {
        static readonly object SyncLock_ = new Object();
        
        readonly IContainer _container;
        readonly IDictionary<IPresenter, ILifetimeScope> _presentersToLifetimeScopes
            = new Dictionary<IPresenter, ILifetimeScope>();

        public AutofacPresenterFactory(IContainer container)
        {
            _container = container;
        }

        public IPresenter Create(Type presenterType, Type viewType, IView view)
        {
            var context = _container.BeginLifetimeScope(_ => {
                _.RegisterType(presenterType);
                _.RegisterInstance((object)view).As(viewType);
            });

            var presenter = (IPresenter)context.Resolve(
                presenterType,
                new LooselyTypedParameter(viewType, view));

            lock (SyncLock_) {
                _presentersToLifetimeScopes[presenter] = context;
            }

            return presenter;
        }

        public void Release(IPresenter presenter)
        {
            var context = _presentersToLifetimeScopes[presenter];

            lock (SyncLock_) {
                _presentersToLifetimeScopes.Remove(presenter);
            }

            // Disposing the container will dispose any of the components
            // created within its lifetime scope.
            context.Dispose();
        }

        class LooselyTypedParameter : ConstantParameter
        {
            public LooselyTypedParameter(Type type, object value)
                : base(value, _ => _.ParameterType.IsAssignableFrom(type))
            {
                if (type == null) { throw new ArgumentNullException("type"); }
            }
        }
    }
}