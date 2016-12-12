// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Diagnostics;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed partial class PresenterBindingAttribute : Attribute
    {
        private readonly Type _presenterType;

        private PresenterBindingMode _bindingMode = PresenterBindingMode.Default;
        private Type _origin;
        private Type _viewType;

        public PresenterBindingAttribute(Type presenterType)
        {
            Require.NotNull(presenterType, nameof(presenterType));

            Debug.Assert(
                typeof(IPresenter<IView>).IsAssignableFrom(presenterType),
                "Asserts 'presenterType' is of type 'IPresenter<IView>'.");

            _presenterType = presenterType;
        }

        public PresenterBindingMode BindingMode
        {
            get { return _bindingMode; }
            set { _bindingMode = value; }
        }

        public Type PresenterType
        {
            get
            {
                Warrant.NotNull<Type>();

                return _presenterType;
            }
        }

        // NB: null values are allowed.
        public Type ViewType
        {
            get { return _viewType; }
            set { _viewType = value; }
        }

        internal Type Origin
        {
            get { return _origin; }
            set { Demand.NotNull(value); _origin = value; }
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Mvp
{
    using System.Diagnostics.Contracts;

    public sealed partial class PresenterBindingAttribute
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_presenterType != null);
        }
    }
}

#endif
