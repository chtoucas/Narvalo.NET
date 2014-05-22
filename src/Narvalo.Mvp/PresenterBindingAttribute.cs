// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class PresenterBindingAttribute : Attribute
    {
        readonly Type _presenterType;

        PresenterBindingMode _bindingMode = PresenterBindingMode.Default;
        Type _origin;
        Type _viewType;

        public PresenterBindingAttribute(Type presenterType)
        {
            Require.NotNull(presenterType, "presenterType");

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
