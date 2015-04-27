// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Diagnostics;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class PresenterBindingAttribute : Attribute
    {
        private readonly Type _presenterType;

        private PresenterBindingMode _bindingMode = PresenterBindingMode.Default;
        private Type _origin;
        private Type _viewType;

        public PresenterBindingAttribute(Type presenterType)
        {
            Require.NotNull(presenterType, "presenterType");

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

        public Type PresenterType { get { return _presenterType; } }

        public Type ViewType
        {
            get { return _viewType; }
            set { _viewType = value; }
        }

        internal Type Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }
    }
}
