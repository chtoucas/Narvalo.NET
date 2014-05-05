// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    /// <summary>
    /// Workaround to enable registration to events not available to the presenter constructor.
    /// It applies to any event occurring before or during a form loading.
    /// Indeed, if a presenter subscribes to one of theses events, the corresponding 
    /// handler will never fire as these events occur before presenter binding.
    /// </summary>
    public interface IFormPresenter : IPresenter
    {
        /// <summary>
        /// Will be automatically called *during* the form Load event
        /// and *after* the presenter has been fully constructed.
        /// </summary>
        void OnBindingComplete();
    }
}
