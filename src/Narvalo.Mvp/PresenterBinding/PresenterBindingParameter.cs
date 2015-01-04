// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo;
    using Narvalo.Mvp.Internal;

    public sealed class PresenterBindingParameter
    {
        readonly Type _presenterType;
        readonly Type _viewType;
        readonly PresenterBindingMode _bindingMode;
        readonly IEnumerable<IView> _views;

        public PresenterBindingParameter(
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

            var other = obj as PresenterBindingParameter;
            if (other == null) {
                return false;
            }

            return PresenterType == other.PresenterType
                && ViewType == other.ViewType
                && BindingMode == other.BindingMode
                && SequenceEqual_(Views, other.Views);
        }

        public override int GetHashCode()
        {
            return PresenterType.GetHashCode()
                | ViewType.GetHashCode()
                | BindingMode.GetHashCode()
                | Views.GetHashCode();
        }

        /// <summary>
        /// An order independent version of Enumerable.SequenceEqual.
        /// </summary>
        static bool SequenceEqual_<T>(IEnumerable<T> left, IEnumerable<T> right)
        {
            Require.NotNull(left, "left");
            Require.NotNull(right, "right");

            var leftObjects = left.ToList();
            var rightObjects = right.ToList();

            if (leftObjects.Count != rightObjects.Count) {
                return false;
            }

            foreach (var item in rightObjects) {
                if (!leftObjects.Contains(item)) {
                    return false;
                }

                leftObjects.Remove(item);
            }

            return leftObjects.IsEmpty();
        }
    }
}
