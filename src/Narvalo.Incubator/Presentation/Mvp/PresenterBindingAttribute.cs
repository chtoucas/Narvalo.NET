namespace Narvalo.Presentation.Mvp
{
	using System;

	/// <summary>
	/// Used to define bindings between presenters and a views.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class PresenterBindingAttribute : Attribute
	{
	    readonly Type _presenterType;
	    
	    Type _viewType;
	    BindingMode _bindingMode = BindingMode.Default;
	    
		/// <summary />
		public PresenterBindingAttribute(Type presenterType)
		{
			_presenterType = presenterType;
		}

        /// <summary />
        public BindingMode BindingMode { get { return _bindingMode; } set { _bindingMode = value; } }

		/// <summary />
		public Type PresenterType { get { return _presenterType; } }

		/// <summary />
		public Type ViewType { get { return _viewType; } set { _viewType = value; } }
	}
}