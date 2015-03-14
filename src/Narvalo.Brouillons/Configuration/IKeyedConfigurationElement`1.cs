﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    public interface IKeyedConfigurationElement<TKey>
    {
        TKey Key { get; }
    }
}