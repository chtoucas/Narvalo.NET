// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Views
{
    using System;
    using Narvalo.Mvp;

    public interface ITestView : IView
    {
        event EventHandler Completed;
    }
}
