﻿// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    public interface IMessageBusFactory
    {
        bool IsStatic { get; }

        IMessageBus Create();
    }
}
