// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground
{
    using System;
    using Narvalo.Mvp;

    public interface ISampleView : IView
    {
        event EventHandler Completed;
    }
}
