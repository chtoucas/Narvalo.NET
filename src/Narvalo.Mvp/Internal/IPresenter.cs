// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Mvp;

    // REVIEW: Hummm, creating this interface was most certainly a bad idea.
    // The idea behind it is that no one except that the PresenterBinder class 
    // can set the message coordinator. This in turn mandates that your
    // presenter inherits one of the three presenter base classes.
    internal interface IPresenter
    {
        [SuppressMessage("Microsoft.Design",
            "CA1044:PropertiesShouldNotBeWriteOnly",
            Justification = "The read part is provided by the public interface.")]
        IMessageCoordinator Messages { set; }
    }
}
