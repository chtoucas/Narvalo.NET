// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms.Internal
{
    using System.Diagnostics.CodeAnalysis;

    public interface IFormPresenter : IPresenter
    {
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        IMessageBus Messages { set; }
    }
}
