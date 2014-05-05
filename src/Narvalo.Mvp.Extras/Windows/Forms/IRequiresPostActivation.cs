// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    // Workaround to replace the Load event for a control.
    // If a presenter registers a Load event handler, it will never fire 
    // as the Load event occurs very early.
    public interface IRequiresActivation
    {
        void OnActivated();
    }
}
