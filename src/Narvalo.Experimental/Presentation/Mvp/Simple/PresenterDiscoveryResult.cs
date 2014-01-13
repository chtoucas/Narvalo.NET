namespace Narvalo.Presentation.Mvp.Simple
{
    using System.Collections.Generic;
    using Narvalo.Collections;

	/// <summary />
	public class PresenterDiscoveryResult
	{
		 readonly IEnumerable<IView> _views;
		 readonly IEnumerable<PresenterBinding> _bindings;

		/// <summary />
		public PresenterDiscoveryResult(
			IEnumerable<IView> views,
            IEnumerable<PresenterBinding> bindings)
		{
			_views = views;
			_bindings = bindings;
		}

		/// <summary />
		public IEnumerable<IView> Views { get { return _views; } }

		/// <summary />
		public IEnumerable<PresenterBinding> Bindings { get { return _bindings; } }

        /// <summary />
        public override bool Equals(object obj)
		{
			var target = obj as PresenterDiscoveryResult;
			if (target == null)
				return false;

			return Views.SetEqual(target.Views)
				&& Bindings.SetEqual(target.Bindings);
		}

        /// <summary />
        public override int GetHashCode()
		{
			return Views.GetHashCode()
				| Bindings.GetHashCode();
		}
	}
}