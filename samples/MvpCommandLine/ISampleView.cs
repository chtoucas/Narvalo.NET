// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpCommandLine
{
    using System;
    using Narvalo.Mvp;

    public interface ISampleView : IView
    {
        event EventHandler Completed;
    }
}
