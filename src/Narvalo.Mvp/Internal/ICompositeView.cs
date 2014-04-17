﻿// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    internal interface ICompositeView : IView
    {
        void Add(IView view);
    }
}