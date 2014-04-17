// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Mvp.Internal;

    public sealed class PresenterBinding
    {
        readonly Type _presenterType;
        readonly Type _viewType;
        readonly PresenterBindingMode _bindingMode;
        readonly IEnumerable<IView> _views;

        public PresenterBinding(
            Type presenterType,
            Type viewType,
            PresenterBindingMode bindingMode,
            IEnumerable<IView> views)
        {
            _presenterType = presenterType;
            _viewType = viewType;
            _bindingMode = bindingMode;
            _views = views;
        }

        public PresenterBindingMode BindingMode { get { return _bindingMode; } }

        public Type PresenterType { get { return _presenterType; } }

        public IEnumerable<IView> Views { get { return _views; } }

        public Type ViewType { get { return _viewType; } }

        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }

            var other = obj as PresenterBinding;
            if (other == null) {
                return false;
            }

            return PresenterType == other.PresenterType
                && ViewType == other.ViewType
                && BindingMode == other.BindingMode
                && WeakEquality.AreEqual(Views, other.Views);
        }

        public override int GetHashCode()
        {
            return PresenterType.GetHashCode()
                | ViewType.GetHashCode()
                | BindingMode.GetHashCode()
                | Views.GetHashCode();
        }
    }
}
