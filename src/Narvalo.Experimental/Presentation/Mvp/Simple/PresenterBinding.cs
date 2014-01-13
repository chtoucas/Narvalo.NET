namespace Narvalo.Presentation.Mvp.Simple
{
    using System;

    /// <summary/>
    public class PresenterBinding
    {
        readonly Type _presenterType;
        readonly Type _viewType;
        readonly IView _view;

        /// <summary/>
        public PresenterBinding(
            Type presenterType,
            Type viewType,
            IView view)
        {
            _presenterType = presenterType;
            _viewType = viewType;
            _view = view;
        }

        /// <summary/>
        public Type PresenterType { get { return _presenterType; } }

        /// <summary/>
        public Type ViewType { get { return _viewType; } }

        /// <summary/>
        public IView View { get { return _view; } }

        /// <summary/>
        public override bool Equals(object obj)
        {
            var target = obj as PresenterBinding;
            if (target == null) {
                return false;
            }

            return _presenterType == target._presenterType
                && _viewType == target._viewType
                && _view == target._view;
        }

        /// <summary/>
        public override int GetHashCode()
        {
            return _presenterType.GetHashCode()
                | _viewType.GetHashCode()
                | _view.GetHashCode();
        }
    }
}