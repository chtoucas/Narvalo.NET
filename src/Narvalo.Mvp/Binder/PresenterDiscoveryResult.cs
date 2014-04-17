// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System.Collections.Generic;
    using Narvalo.Mvp.Internal;

    public sealed class PresenterDiscoveryResult
    {
        readonly IEnumerable<IView> _views;
        readonly IEnumerable<PresenterBinding> _bindings;

        public PresenterDiscoveryResult(
            IEnumerable<IView> views,
            IEnumerable<PresenterBinding> bindings)
        {
            _views = views;
            _bindings = bindings;
        }

        public IEnumerable<IView> Views { get { return _views; } }

        public IEnumerable<PresenterBinding> Bindings { get { return _bindings; } }

        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }

            var other = obj as PresenterDiscoveryResult;
            if (other == null) {
                return false;
            }

            return WeakEquality.AreEqual(Views, other.Views)
                && WeakEquality.AreEqual(Bindings, other.Bindings);
        }

        public override int GetHashCode()
        {
            return Views.GetHashCode()
                | Bindings.GetHashCode();
        }
    }
}