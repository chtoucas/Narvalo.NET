// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Collections.Generic;
#if CONTRACTS_FULL // Contract Class and Object Invariants.
    using System.Diagnostics.Contracts;
#endif
    using System.Linq;

    using Narvalo;
    using Narvalo.Mvp.Internal;

    using static System.Diagnostics.Contracts.Contract;

    public sealed class PresenterBindingParameter
    {
        private readonly Type _presenterType;
        private readonly Type _viewType;
        private readonly PresenterBindingMode _bindingMode;
        private readonly IEnumerable<IView> _views;

        public PresenterBindingParameter(
            Type presenterType,
            Type viewType,
            PresenterBindingMode bindingMode,
            IEnumerable<IView> views)
        {
            Require.NotNull(presenterType, nameof(presenterType));
            Require.NotNull(viewType, nameof(viewType));
            Require.NotNull(views, nameof(views));

            _presenterType = presenterType;
            _viewType = viewType;
            _bindingMode = bindingMode;
            _views = views;
        }

        public PresenterBindingMode BindingMode { get { return _bindingMode; } }

        public Type PresenterType
        {
            get
            {
                Ensures(Result<Type>() != null);

                return _presenterType;
            }
        }

        public IEnumerable<IView> Views
        {
            get
            {
                Ensures(Result<IEnumerable<IView>>() != null);

                return _views;
            }
        }

        public Type ViewType
        {
            get
            {
                Ensures(Result<Type>() != null);

                return _viewType;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var other = obj as PresenterBindingParameter;
            if (other == null)
            {
                return false;
            }

            return PresenterType == other.PresenterType
                && ViewType == other.ViewType
                && BindingMode == other.BindingMode
                && SequenceEqual(Views, other.Views);
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
        private static bool SequenceEqual<T>(IEnumerable<T> left, IEnumerable<T> right)
        {
            Require.NotNull(left, nameof(left));
            Require.NotNull(right, nameof(right));

            var leftObjects = left.ToList();
            var rightObjects = right.ToList();

            if (leftObjects.Count != rightObjects.Count)
            {
                return false;
            }

            foreach (var item in rightObjects)
            {
                if (!leftObjects.Contains(item))
                {
                    return false;
                }

                leftObjects.Remove(item);
            }

            return leftObjects.IsEmpty();
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Invariant(_presenterType != null);
            Invariant(_views != null);
            Invariant(_viewType != null);
        }

#endif
    }
}
