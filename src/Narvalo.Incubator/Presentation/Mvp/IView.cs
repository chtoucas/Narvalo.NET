namespace Narvalo.Presentation.Mvp
{
	using System;

	/// <summary>
	/// Represents a class that is a view in a Web Forms MVP application.
	/// </summary>
	public interface IView
	{
		/// <summary />
		bool ThrowIfNoPresenterBound { get; }

		/// <summary>
		/// Occurs at the discretion of the view. For <see cref="MvpUserControl"/>
		/// implementations (the most commonly used), this is fired duing the ASP.NET
		/// Load event.
		/// </summary>
		event EventHandler Load;
	}
}